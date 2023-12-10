using At.Lagg.ActivityWatchVS2022.API.V1;
using At.Lagg.ActivityWatchVS2022.API.V1.DataObj;
using At.Lagg.ActivityWatchVS2022.VO;
using Microsoft;
using Microsoft.VisualStudio.Extensibility;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace At.Lagg.ActivityWatchVS2022.Services
{
    public class EventService : ExtensionPart
    {
        private const int WORKER_LOOP_MS = 5000;
        private const int WORKER_SHUTDOWN_MS = 30000;
        private const int EVENT_AFK_SECONDS = 60 * 15;
        private const string API_CLIENT_NAME = "aw-watcher-vs2022";
        private const string AW_HOMEPAGE = @"https://activitywatch.net/";
        private const int SEND_RETRY_MS = 10000;


        private readonly ConsoleService _consoleService;
        private readonly SolutionInfoService _solutionInfoService;
        private ConcurrentQueue<VsEventInfo> _eventsQueue = new ConcurrentQueue<VsEventInfo>();
        private static bool _doShutdown = false;
        private Task? _workerThread = null;
        private volatile object _lock = new object();
        private HashSet<string> _sentBuckets = new HashSet<string>();
        private readonly AutoResetEvent _continueWorker = new AutoResetEvent(false);
        private Client _client;

        public EventService(ExtensionCore container, VisualStudioExtensibility extensibility, ConsoleService consoleService, SolutionInfoService solutionInfoService
        ) : base(container, extensibility)
        {
            this._consoleService = Requires.NotNull(consoleService, nameof(consoleService));
            this._solutionInfoService = Requires.NotNull(solutionInfoService, nameof(solutionInfoService));

            initClient();
        }

        /// <summary>
        /// Productive or Testing Server?
        /// </summary>
        private bool IsProductive
        {
            get
            {
#if DEBUG
                return false;
#else
                return true;
#endif
            }
        }

        private int ApiPort
        {
            get
            {
                return IsProductive ? 5600 : 5666;
            }
        }


        private void initClient()
        {
            _client = new API.V1.Client();

            //Todo should be configurable once out-of-proc supports Options dialog
            _client.BaseUrl = $@"http://localhost:{ApiPort}/api";
        }

        public async Task AddEventAsync(Microsoft.VisualStudio.RpcContracts.Editor.TextViewContract textView, [CallerMemberName] string? caller = null)
        {
            VO.VsEventInfo eventInfo = new VO.VsEventInfo();
            eventInfo.UtcEventDateTime = DateTime.UtcNow;
            eventInfo.ChangedFile = textView.Document?.Uri?.AbsolutePath ?? string.Empty;
            eventInfo.Caller = caller ?? string.Empty;
            eventInfo.SolutionInfo = await _solutionInfoService.GetSolutionInfoAsync();
            _eventsQueue.Enqueue(eventInfo);
            startQueue();

            _consoleService.WriteLineDebug("queued change in '{0}', event source {1}, in solution {2} ({3})",
                eventInfo.ChangedFile, caller, eventInfo.SolutionInfo?.BaseName, eventInfo.SolutionInfo?.Path);
        }

        private void startQueue()
        {
            if (_workerThread == null)
                lock (_lock)
                    if (_workerThread == null)
                    {
                        _workerThread = Task.Run(() => workerThread());
                    }
            _continueWorker.Set();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                finishQueue();
            }
            base.Dispose(isDisposing);
        }

        private void finishQueue()
        {
            _doShutdown = true;
            _continueWorker.Set();
        }

        private async Task workerThread()
        {
            API.V1.Event? unsentEvent = null;
            for (; ; )
            {
                try
                {
                    _continueWorker.WaitOne(WORKER_LOOP_MS);

                    // deque new event and work on it
                    while (_eventsQueue.TryDequeue(out VsEventInfo evInfo))
                    {
                        API.V1.Event? newEvent = null;
                        if (unsentEvent == null)
                        {
                            // queue has no event to build upon
                            newEvent = (API.V1.Event)evInfo;
                        }
                        else
                        {
                            // we have a current event, and will extend it.
                            newEvent = unsentEvent.ExtendOrCreate(evInfo);
                        }

                        if (newEvent != null)
                        {
                            // we have a new event, so send the old unsent one
                            await sendAsync(unsentEvent);

                            // then keep the new event as unsent
                            unsentEvent = newEvent;
                        }
                    }

                    // send event when timeout reached
                    if (unsentEvent?.EventTimeouted(EVENT_AFK_SECONDS) ?? false)
                    {
                        await sendAsync(unsentEvent);
                        unsentEvent = null;
                    }

                    // if stopped, finish last event end end
                    if (_doShutdown)
                    {
                        if (unsentEvent != null)
                        {
                            unsentEvent.SetDuration();
                            await sendAsync(unsentEvent);

                        }
                        unsentEvent = null;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    //TODO: log full exception + inner exceptions
                    this._consoleService.WriteLine(
                        Microsoft.Extensions.Logging.LogLevel.Error,
                     "{0}: {1}", nameof(workerThread), ex.Message
                    );
                }
            }
        }

        private async Task sendAsync(Event? unsentEvent)
        {
            if (unsentEvent == null)
            {
                return;
            }

            // retry until success 
            do
            {
                try
                {
                    await internalSendBucketAsync(unsentEvent);
                    await internalSendEventAsync(unsentEvent);
                    unsentEvent = null;
                }
                catch (Exception ex)
                {
                    _consoleService.WriteLineError($"Is ActivityWatch installed and running? see {AW_HOMEPAGE}\r\n\tError {ex.Message}");
                    await Task.Delay(SEND_RETRY_MS);
                }
            } while (unsentEvent != null);
        }

        private async Task internalSendEventAsync(Event unsentEvent)
        {
            string bucketID = unsentEvent.GetBucketID();
            await _client.BucketsEventsPostAsync(unsentEvent, bucketID);
            _consoleService.WriteLineDebug($"{unsentEvent.GetBucketID()}: sent event {unsentEvent}.");
        }

        private async Task internalSendBucketAsync(Event ev)
        {

            IDataObj evData = ev.IDataObj ?? throw new ArgumentNullException(nameof(ev));
            string bucketID = ev.GetBucketID();

            if (_sentBuckets.Contains(bucketID))
            {
                return;
            }

            try
            {
                var payload = new API.V1.CreateBucket()
                {
                    Client = API_CLIENT_NAME,
                    Hostname = Environment.MachineName,
                    Type = ev.IDataObj.TypeName
                };

                AWResponse? result = await _client.BucketsPostAsync(payload, bucketID);
                _sentBuckets.Add(bucketID);
            }
            catch (AWApiException apiEx) when (apiEx.StatusCode == 304)
            {
                // bucket already exists
                _sentBuckets.Add(bucketID);
            }
        }
    }
}
