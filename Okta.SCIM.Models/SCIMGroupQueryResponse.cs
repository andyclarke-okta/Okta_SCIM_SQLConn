using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okta.SCIM.Models
{
    public class SCIMGroupQueryResponse : SCIMResourceQueryResponse
    {
        public List<SCIMGroup> Resources { get; set; }
    }
}
