using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCPUtilityAzureFunction.Models
{
    public class FileListData
    {
        public class FileDetails
        {            
            public string Name { get; set; }
            [JsonProperty(PropertyName = "OBID")]
            public string ObId { get; set; }
        };
        [JsonProperty(PropertyName = "SPFFileComposition_21 @odata.count")]
        public int Count { get; set; }
        [JsonProperty(PropertyName = "SPFFileComposition_21")]
        public List<FileDetails> fileDetails { get; set; }
    }
}
