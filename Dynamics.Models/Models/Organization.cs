using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models
{
    public class Organization
    {
		public int OrganizationID { get; set; }
		public string OrganizationName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? OrganizationEmail { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? OrganizationPhoneNumber { get; set; }
        public string? OrganizationAddress { get; set; }
        public string OrganizationDescription { get; set; }
		public string? OrganizationPictures { get; set; }
        [DataType(DataType.Date)]
        public DateOnly StartTime { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? ShutdownDay { get; set; }
		public string? CEOID { get; set; }

		public virtual ICollection<Project> Project { get; set; }
		public virtual ICollection<OrganizationMember> OrganizationMember { get; set; }
		public virtual ICollection<OrganizationResource> OrganizationResource { get; set; }
		public virtual ICollection<OrganizationToProjectHistory> OrganizationToProjectTransactions { get; set; }
		public virtual ICollection<UserToOrganizationTransactionHistory> UserToOrganizationTransactions { get; set; }
	}
}
