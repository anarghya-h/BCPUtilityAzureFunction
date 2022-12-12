using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BCPUtilityAzureFunction.Models
{
    public class ODataQueryResponse
    {
        [JsonProperty(PropertyName = "@odata.count")]
        public int Count { get; set; }
        [JsonProperty(PropertyName = "value")]
        public List<BCPDocData> Value { get; set; }
        [JsonProperty(PropertyName = "@odata.nextLink")]
        public string NextLink { get; set; }
    }
}

