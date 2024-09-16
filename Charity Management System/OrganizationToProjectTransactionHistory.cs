using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charity_Management_System
{
    public class OrganizationToProjectTransactionHistory
    {
        public int transactionID { get; set; }
        public int organizationID { get; set; }
        public int projectID { get; set; }
        public string message { get; set; }
        public DateTime time { get; set; }
        public virtual Organization organization { get; set; }
        public virtual Project project { get; set; }
    }
}
