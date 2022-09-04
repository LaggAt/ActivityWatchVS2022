using At.Lagg.ActivityWatchVS2022.API.V1.DataObj;
using At.Lagg.ActivityWatchVS2022.VO;

namespace At.Lagg.ActivityWatchVS2022.API.V1
{
    internal partial class Event : IEquatable<Event>
    {
        #region Properties

        [Newtonsoft.Json.JsonIgnore()]
        public IDataObj? IDataObj
        { get { return this.Data as IDataObj; } }

        #endregion Properties

        #region Methods

        public static implicit operator Event(VsEventInfo v)
        {
            string file = v.ChangedFile;
            string solution = string.Empty;
            if (v.SolutionInfo != null)
            {
                string rootPath = v.SolutionInfo.Value.Directory.Replace('\\', '/');
                solution = $"{v.SolutionInfo.Value.BaseName} ({v.SolutionInfo.Value.Directory})";
                if (file.StartsWith(rootPath, StringComparison.Ordinal))
                {
                    file = $"./{file.Substring(rootPath.Length)}";
                }
            }

            var data = new DataObj.AppEditorActivity()
            {
                Caller = v.Caller,
                File = file,
                Language = Path.GetExtension(file).TrimStart(".".ToCharArray()),
                Project = solution,
            };
            return new Event()
            {
                Timestamp = v.UtcEventDateTime,
                Data = data,
            };
        }

        public override bool Equals(object? other)
        {
            if (other is Event ev)
            {
                return this.Equals(ev);
            }
            return base.Equals(other);
        }

        /// <summary>
        /// used to extend events of same type/file/...
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(Event? other)
        {
            if (other == null) return false; // no other
            if (this.Data == null && other.Data == null) return true; // both data null
            if (this.Data == null) return false; // just this data is null

            switch (this.Data)
            {
                case AppEditorActivity appEditorActivity:
                    return appEditorActivity.Equals(other.Data);

                default:
                    return this.Data?.Equals(other?.Data) ?? false;
            }
        }

        public override string ToString()
        {
            return $"starting {this.Timestamp} for {this.Duration} seconds: {this.Data}";
        }

        /// <summary>
        /// Extends a similar event and returns null, OR creates a new event and returns it
        /// </summary>
        /// <param name="evInfo"></param>
        /// <returns></returns>
        internal Event? ExtendOrCreate(VsEventInfo evInfo)
        {
            Event newEvent = (API.V1.Event)evInfo;
            if (this.Equals(newEvent))
            {
                // extend previous event, do not return the new one
                SetDuration();
                return null;
            }
            SetDuration(newEvent);
            return newEvent;
        }

        /// <summary>
        /// End this event with UTCNow.
        /// </summary>
        /// <returns></returns>
        internal void SetDuration(Event? nextEvent = null)
        {
            DateTimeOffset until = DateTimeOffset.UtcNow;
            if (nextEvent != null)
            {
                until = nextEvent.Timestamp;
            }
            this.Duration = (double)(until - this.Timestamp).TotalSeconds;
        }

        /// <summary>
        /// Checks if the event is old enough to time out, and set's the duration if so.
        /// </summary>
        /// <param name="afkSeconds"></param>
        /// <returns></returns>
        internal bool EventTimeouted(int afkSeconds)
        {
            this.SetDuration();
            return this.Duration > afkSeconds;
        }

        internal string GetBucketID()
        {
            switch (this.Data)
            {
                case IDataObj iData:
                    return $"{iData.BucketIDCustomPart}_{Environment.MachineName}";

                default:
                    throw new NotImplementedException(this.Data?.GetType().ToString());
            }
        }

        #endregion Methods
    }
}
