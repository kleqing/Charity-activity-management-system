using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models
{
    public class Organization
    {
		public Guid OrganizationID { get; set; }
		public string OrganizationName { get; set; }
		public string OrganizationDescription { get; set; }
		public string OrganizationPictures { get; set; }
		[DataType(DataType.Date)]
        public DateOnly StartTime { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? ShutdownDay { get; set; }
		public Guid? CEOID { get; set; }
		public bool isBanned { get; set; }
        [NotMapped]
        public int ProjectCount { get; set; }
        public virtual ICollection<Project> Project { get; set; }
		public virtual ICollection<OrganizationMember> OrganizationMember { get; set; }
		public virtual ICollection<OrganizationResource> OrganizationResource { get; set; }
		public virtual ICollection<OrganizationToProjectHistory> OrganizationToProjectTransactions { get; set; }
		public virtual ICollection<UserToOrganizationHistory> UserToOrganizationTransactions { get; set; }
	}
}
