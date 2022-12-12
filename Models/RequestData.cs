using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCPUtilityAzureFunction.Models
{
    public class RequestData
    {
        public string pstrNotificationOBID { get; set; }
        public bool isMergedRendition { get; set; }
    }
}
