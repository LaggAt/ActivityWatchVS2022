using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace At.Lagg.ActivityWatchVS2022.API.V1.DataObj
{
    internal class AppEditorActivity
    {
        [Newtonsoft.Json.JsonIgnore]
        public string BucketIDCustomPart { get => "aw-visualstudio-editor"; }

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

        [Newtonsoft.Json.JsonIgnore]
        public string TypeName { get => "app.editor.activity"; }
    }
}
