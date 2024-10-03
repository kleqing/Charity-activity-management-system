using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models
{
    public class Project
    {
		public Guid ProjectID { get; set; }
		public Guid OrganizationID { get; set; }
		public Guid? RequestID { get; set; }
        [Required]
		public string ProjectName { get; set; }
        [Required]
        public int ProjectStatus { get; set; }
		[ValidateNever]
        public string? Attachment { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        [DataType(DataType.EmailAddress)]
        public  string? Email { get; set; }
        public string? ReportFile { get; set; }
        public string? ProjectDescription { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? StartTime { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? EndTime { get; set; }
        public string? Reason { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual Request Request { get; set; }
		public virtual ICollection<ProjectMember> ProjectMember { get; set; }
		public virtual ICollection<ProjectResource> ProjectResource { get; set; }
		public virtual ICollection<History> History { get; set; }
        public virtual ICollection<UserToProjectTransactionHistory> UserToProjectTransactions { get; set; }
		public virtual ICollection<OrganizationToProjectHistory> OrganizationToProjectTransactions { get; set; }
	}
}
