using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCPUtilityAzureFunction.Models
{
    public class NotificationFileData
    {
        public string Name { get; set; }
        [JsonProperty(PropertyName = "URL")]
        public string Url { get; set; }
        public string ContentLength { get; set; }
        public string FileOBID { get; set; }
        public string ParentFileOBID { get; set; }
    }
}
