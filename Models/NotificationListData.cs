using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCPUtilityAzureFunction.Models
{
    public class NotificationListData
    {
        public string Obid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
