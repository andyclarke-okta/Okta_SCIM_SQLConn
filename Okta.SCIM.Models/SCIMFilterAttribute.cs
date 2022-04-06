using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okta.SCIM.Models
{
    public class SCIMFilterAttribute
    {
        public String	AttributeName { get; set;}

        public String Schema {get; set;}

        public String SubAttributeName { get; set; }
    }
}
