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

        [Required(ErrorMessage = "The Project Name field is required *")]
        [MaxLength(100, ErrorMessage = "Project Name length cannot be longer than 100 characters.")]
        public string ProjectName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? ProjectEmail { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? ProjectPhoneNumber { get; set; }
        public string? ProjectAddress { get; set; }
        public int ProjectStatus { get; set; }
        public string? Attachment { get; set; }

        [Required(ErrorMessage = "The Organization Name field is required *")]
        public string ProjectDescription { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? StartTime { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? EndTime { get; set; }
        public string? shutdownReanson { get; set; }
		public virtual Organization Organization { get; set; }
		public virtual Request Request { get; set; }
		public virtual ICollection<ProjectMember> ProjectMember { get; set; }
		public virtual ICollection<ProjectResource> ProjectResource { get; set; }
		public virtual ICollection<History> History { get; set; }
	}
}
