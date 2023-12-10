using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace At.Lagg.ActivityWatchVS2022.API.V1.DataObj
{
    internal class AppEditorActivity : IDataObj, IEquatable<AppEditorActivity>
    {
        [Newtonsoft.Json.JsonIgnore]
        public string BucketIDCustomPart { get => "aw-visualstudio-editor"; }

        [Newtonsoft.Json.JsonIgnore]
        public string TypeName { get => "app.editor.activity"; }


        [Newtonsoft.Json.JsonProperty("caller", Required = Newtonsoft.Json.Required.Default)]
        public string Caller { get; set; }

        [Newtonsoft.Json.JsonProperty("file", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string File { get; set; }

        [Newtonsoft.Json.JsonProperty("language", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Language { get; set; }

        [Newtonsoft.Json.JsonProperty("project", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Project { get; set; }

        public override bool Equals(object? other)
        {
            if(other is AppEditorActivity appEditorActivity)
            {
                return this.Equals(appEditorActivity);
            }
            return base.Equals(other);
        }

        /// <summary>
        /// used to extend events of same file/...
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(AppEditorActivity? other)
        {
            if(other == null)
            {
                return false;
            }

            return this.File == other.File &&
                this.Language == other.Language &&
                this.Project == other.Project &&
                this.Caller == other.Caller;
        }

        public override string ToString()
        {
            return $"{this.File} changed by {this.Caller}";
        }
    }
}
