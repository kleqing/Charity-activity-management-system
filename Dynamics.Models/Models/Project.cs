using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
		public string ProjectName { get; set; }
		public int ProjectStatus { get; set; }
        public string Attachment { get; set; }
		public string ProjectDescription { get; set; }
        [DataType(DataType.Date)]
        public DateOnly StartTime { get; set; }
		[DataType(DataType.Date)]
        public DateOnly? EndTime { get; set; }
        public Guid? LeaderID { get; set; }
		public virtual Organization Organization { get; set; }
		public virtual Request Request { get; set; }
		public virtual ICollection<ProjectMember> ProjectMember { get; set; }
		public virtual ICollection<ProjectResource> ProjectResource { get; set; }
		public virtual ICollection<History> History { get; set; }
        public virtual ICollection<UserToProjectHistory> UserToProjectTransactions { get; set; }
		public virtual ICollection<OrganizationToProjectHistory> OrganizationToProjectTransactions { get; set; }
	}
}
