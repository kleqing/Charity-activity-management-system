﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject
{
    public class Project
    {
		public int projectID { get; set; }
		public int organizationID { get; set; }
		public int? requestID { get; set; }
		public string projectName { get; set; }
		public string attachment { get; set; }
		public string projectDescription { get; set; }
		public DateOnly? startTime { get; set; }
		public DateOnly? endTime { get; set; }
		public int? leaderID { get; set; }
		public virtual Organization Organization { get; set; }
		public virtual Request Request { get; set; }
		public virtual ICollection<ProjectMember> ProjectMember { get; set; }
		public virtual ICollection<ProjectResource> ProjectResource { get; set; }
		public virtual ICollection<UserToProjectTransactionHistory> UserToProjectTransactions { get; set; }
		public virtual ICollection<OrganizationToProjectTransactionHistory> OrganizationToProjectTransactions { get; set; }
	}
}
