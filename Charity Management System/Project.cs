using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charity_Management_System
{
    public class Project
    {
        public int projectID { get; set; }
        public string projectName { get; set; }
        public string attachment { get; set; }
        public string projectDescription { get; set; }
        public DateTime startTime { get; set; }
        public DateTime? endTime { get; set; }
        public int leaderID { get; set; }
        public int organizationID { get; set; }
        public int requestID { get; set; }
        public virtual ICollection<ProjectMember> projectMember { get; set; }
        public virtual ICollection<ProjectResource> projectResource { get; set; }
        public virtual ICollection<Request> request { get; set; }
        public virtual Organization organization { get; set; }
        public virtual ICollection<OrganizationToProjectTransactionHistory>? organizationToProjectTransactionHistory { get; set; }
        public virtual ICollection<UserToProjectTransactionHistory>? userToProjectTransactionHistory { get; set; }
	}
}
