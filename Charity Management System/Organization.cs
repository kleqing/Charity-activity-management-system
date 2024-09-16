using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charity_Management_System
{
    public class Organization
    {
        public int organizationID { get; set; }
        public string organizationName { get; set; }
        public string organizationDescription { get; set; }
        public string attachment { get; set; }
        public DateTime startTime { get; set; }
        public DateTime? shutdownDay { get; set; }  // Nullable
        public int ceoID { get; set; }
        public virtual Project project { get; set; }
        public virtual OrganizationResource organizationResource { get; set; }
        public virtual OrganizationMember organizationMember { get; set; }
        public virtual OrganizationToProjectTransactionHistory organizationToProjectTransactionHistory { get; set; }
        public virtual UserToOrganizationTransactionHistory userToOrganizationTransactionHistory { get; set; }
    }
}
