using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models
{
    public class Organization
    {
		public int organizationID { get; set; }
		public string organizationName { get; set; }
		public string organizationDescription { get; set; }
		public string attachment { get; set; }
		public DateOnly? startTime { get; set; }
		public DateOnly? shutdownDay { get; set; }
		public int? ceoID { get; set; }
		public virtual ICollection<Project> Project { get; set; }
		public virtual ICollection<OrganizationMember> OrganizationMember { get; set; }
		public virtual ICollection<OrganizationResource> OrganizationResource { get; set; }
		public virtual ICollection<OrganizationToProjectTransactionHistory> OrganizationToProjectTransactions { get; set; }
		public virtual ICollection<UserToOrganizationTransactionHistory> UserToOrganizationTransactions { get; set; }
	}
}
