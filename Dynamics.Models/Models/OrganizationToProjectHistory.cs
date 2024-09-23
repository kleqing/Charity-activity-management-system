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
		public int TransactionID { get; set; }
		public int OrganizationID { get; set; }
		public int ProjectID { get; set; }
		public string ResourceName { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
		[DataType(DataType.Date)]
		public DateOnly Time { get; set; }
		public string Amount { get; set; }
		public int Unit { get; set; }
        public virtual Organization Organization { get; set; }
		public virtual Project Project { get; set; }
	}
}
