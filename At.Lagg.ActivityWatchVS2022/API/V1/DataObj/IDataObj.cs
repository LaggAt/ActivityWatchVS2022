using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace At.Lagg.ActivityWatchVS2022.API.V1.DataObj
{
    internal interface IDataObj
    {
        string BucketIDCustomPart { get; }
        string TypeName { get; }
    }
}
