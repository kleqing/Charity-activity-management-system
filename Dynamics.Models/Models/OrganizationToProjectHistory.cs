using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models
{
    public class OrganizationToProjectHistory
    {
		public Guid TransactionID { get; set; }
		public Guid OrganizationID { get; set; }
		public Guid ProjectID { get; set; }
        public int Status { get; set; }
        public string? Message { get; set; }
        public Guid? ResourceID { get; set; }
		public ProjectResource? Resource { get; set; }
        [DataType(DataType.Date)]
		public DateTime? Time { get; set; }
		public int Amount { get; set; }
        public virtual Organization Organization { get; set; }
		public virtual Project Project { get; set; }
	}
}
