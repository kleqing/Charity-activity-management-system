using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
		public Guid OrganizationID { get; set; }
		[ValidateNever]
		public string OrganizationName { get; set; }
        [ValidateNever]
        public string? OrganizationDescription { get; set; }
		public string? OrganizationPictures { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? StartTime { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? ShutdownDay { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public virtual ICollection<Project> Project { get; set; }
        public virtual ICollection<OrganizationMember> OrganizationMember { get; set; }
		public virtual ICollection<OrganizationResource> OrganizationResource { get; set; }
		public virtual ICollection<OrganizationToProjectHistory> OrganizationToProjectTransactions { get; set; }
		public virtual ICollection<UserToOrganizationTransactionHistory> UserToOrganizationTransactions { get; set; }
	}
}
