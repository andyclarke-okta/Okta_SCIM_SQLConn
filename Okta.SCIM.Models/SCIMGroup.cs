using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Okta.SCIM.Models
{
   

    public class Member
    {
        public string value { get; set; }
        public string display { get; set; }
    }

    public class SCIMGroup : SCIMResource
    {
        public string displayName { get; set; }
        public List<Member> members { get; set; }
    }
}
