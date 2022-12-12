using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCPUtilityAzureFunction.Models.Configs
{
    public class SdxConfig
    {
        public string ServerBaseUri { get; set; }
        public string WebClientBaseUri { get; set; }
        public string ServerResourceID { get; set; }
        public string AuthServerAuthority { get; set; }
        public string AuthClientId { get; set; }
        public string AuthClientSecret { get; set; }
        public string OnBehalfOfUser { get; set; }
    }
}
