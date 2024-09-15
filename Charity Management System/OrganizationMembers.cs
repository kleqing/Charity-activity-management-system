using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charity_Management_System
{
    public class OrganizationMember
    {
        public int userID { get; set; }
        public int organizationID { get; set; }
        public virtual User user { get; set; }
        public virtual Organization organization { get; set; }
    }
}
