using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okta.SCIM.Models
{
    public class SCIMUserQueryResponse : SCIMResourceQueryResponse
    {
        public List<SCIMUser> Resources { get; set; }

    }
}
