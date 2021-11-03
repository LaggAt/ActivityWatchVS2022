using At.Lagg.ActivityWatchVS2022.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace At.Lagg.ActivityWatchVS2022.API.V1
{
    internal partial class Event
    {
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
            return new Event(data, 0, v.UtcEventDateTime);
        }

        /// <summary>
        /// Extends a similar event and returns null, OR
        /// creates a new event and returns it
        /// </summary>
        /// <param name="evInfo"></param>
        /// <returns></returns>
        internal Event? ExtendOrCreate(VsEventInfo evInfo)
        {
            Event newEvent = (API.V1.Event)evInfo;
            if (this.Equals(newEvent))
            {
                //TODO: extend event
                throw new NotImplementedException();
                return null;
            }
            return newEvent;
        }

        /// <summary>
        /// End this event with UTCNow.
        /// </summary>
        /// <returns></returns>
        internal bool EndEvent()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if the event is old enough to time out, and set's the duration if so.
        /// </summary>
        /// <param name="afkSeconds"></param>
        /// <returns></returns>
        internal bool EventTimeouted(int afkSeconds)
        {
            throw new NotImplementedException();
        }
    }
}
