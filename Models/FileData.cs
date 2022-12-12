using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCPUtilityAzureFunction.Models
{
    public class FileData
    {
        public string FileId { get; set; }
        public string Id { get; set; }
        public string ItemType { get; set; }
        public string Purpose { get; set; }
        public string Uri { get; set; }
        public string ContentLength { get; set; }
        public string ParentFileOBID { get; set; }

    }
}
