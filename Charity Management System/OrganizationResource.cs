using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charity_Management_System
{
    public class OrganizationResource
    {
        public int resourceID { get; set; }
        public string resourceName { get; set; }
        public int organizationID { get; set; }
        public int quantity { get; set; }
        public string unit { get; set; }
        public string contentTransaction { get; set; }
        public virtual Organization organization { get; set; }
    }
}
