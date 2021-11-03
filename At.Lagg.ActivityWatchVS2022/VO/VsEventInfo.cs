using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace At.Lagg.ActivityWatchVS2022.VO
{
    public record struct VsEventInfo(DateTime UtcEventDateTime, string ChangedFile, string Caller, SolutionInfo? SolutionInfo);
}
