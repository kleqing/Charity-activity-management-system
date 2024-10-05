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
		
        [DataType(DataType.EmailAddress)]
        public string ProjectEmail { get; set; }
        
        [DataType(DataType.PhoneNumber)]
        public string? ProjectPhoneNumber { get; set; }
        
        public string? ProjectAddress { get; set; }
		public string ProjectDescription { get; set; }
        [Required]
        public int ProjectStatus { get; set; }
		[ValidateNever]
        public string? Attachment { get; set; }
        public string? ReportFile { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? StartTime { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? EndTime { get; set; }
        public string? ShutdownReason { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual Request Request { get; set; }
		public virtual ICollection<ProjectMember> ProjectMember { get; set; }
		public virtual ICollection<ProjectResource> ProjectResource { get; set; }
		public virtual ICollection<History> History { get; set; }
	}
}
