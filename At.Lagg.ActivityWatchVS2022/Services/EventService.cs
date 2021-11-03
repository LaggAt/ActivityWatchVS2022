using At.Lagg.ActivityWatchVS2022.API.V1;
using At.Lagg.ActivityWatchVS2022.VO;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Editor;
using Microsoft.VisualStudio.ProjectSystem.Query;
using Microsoft.VisualStudio.ProjectSystem.Query.ProjectModel;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;

namespace At.Lagg.ActivityWatchVS2022.Services
{
    public class EventService : ExtensionPart
    {
        private const int WORKER_LOOP_MS = 5000;
        private const int WORKER_SHUTDOWN_MS = 30000;
        private const int EVENT_AFK_SECONDS = 60 * 15;

        private readonly ConsoleService _consoleService;
        private readonly SolutionInfoService _solutionInfoService;
        private ConcurrentQueue<VsEventInfo> _eventsQueue = new ConcurrentQueue<VsEventInfo>();
        private static bool _doShutdown = false;
        private Task? _workerThread = null;
        private volatile object _lock = new object();
        private readonly AutoResetEvent _continueWorker = new AutoResetEvent(false);

        public EventService(ExtensionCore container, VisualStudioExtensibility extensibility, ConsoleService consoleService, SolutionInfoService solutionInfoService
        ) : base(container, extensibility)
        {
            this._consoleService = Requires.NotNull(consoleService, nameof(consoleService));
            this._solutionInfoService = Requires.NotNull(solutionInfoService, nameof(solutionInfoService));
        }

        public async Task AddEventAsync(Microsoft.VisualStudio.RpcContracts.Editor.TextView textView, [CallerMemberName] string? caller = null)
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
                        API.V1.Event? newEvent = unsentEvent?.ExtendOrCreate(evInfo) ?? (API.V1.Event)evInfo;
                        if (newEvent != null)
                        {
                            // we have a new event, so send the old unsent one
                            await sendEventAsync(unsentEvent);

                            // then keep the new event as unsent
                            unsentEvent = newEvent;
                        }
                    }

                    // send event when timeout reached
                    if (unsentEvent?.EventTimeouted(EVENT_AFK_SECONDS) ?? false)
                    {
                        await sendEventAsync(unsentEvent);
                        unsentEvent = null;
                    }

                    // if stopped, finish last event end end
                    if (_doShutdown)
                    {
                        if (unsentEvent?.EndEvent() ?? false)
                        {
                            await sendEventAsync(unsentEvent);

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

        private async Task sendEventAsync(Event? unsentEvent)
        {
            if(unsentEvent == null)
            {
                return;
            }

            throw new NotImplementedException();
        }
    }
}
