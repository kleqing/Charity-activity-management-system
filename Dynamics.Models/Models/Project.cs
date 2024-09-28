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
		public int ProjectID { get; set; }
		public int OrganizationID { get; set; }
		public int? RequestID { get; set; }
		public string ProjectName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string ProjectEmail { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string ProjectPhone { get; set; }
        public string? ProjectAddress { get; set; }
        public int ProjectStatus { get; set; }
        public string? Attachment { get; set; }
		public string ProjectDescription { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? StartTime { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? EndTime { get; set; }
		public string LeaderID { get; set; }
		public virtual Organization Organization { get; set; }
		public virtual Request Request { get; set; }
		public virtual ICollection<ProjectMember> ProjectMember { get; set; }
		public virtual ICollection<ProjectResource> ProjectResource { get; set; }
		public virtual ICollection<History> History { get; set; }
        public virtual ICollection<UserToProjectTransactionHistory> UserToProjectTransactions { get; set; }
		public virtual ICollection<OrganizationToProjectHistory> OrganizationToProjectTransactions { get; set; }
	}
}
