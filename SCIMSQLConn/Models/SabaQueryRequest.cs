using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SfdcOPPConn.Models
{
    public class SabaSetActive
    {
        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("started_on")]
        public string started_on { get; set; }
    }
    //
    public class SabaSetTerminated
    {
        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("terminated_on")]
        public string terminated_on { get; set; }

        [JsonProperty("username")]
        public string username { get; set; }
    }

    public class SabaSetManager
    {
        [JsonProperty("person")]
        public SabaPersonId person { get; set; }

        [JsonProperty("alternateManagers")]
        public List<SabaPersonId> alternateManagers { get; set; }
    }

    public class SabaPersonId
    {
        [JsonProperty("id")]
        public string  id { get; set; }
    //    public string displayName { get; set; }
    }


}