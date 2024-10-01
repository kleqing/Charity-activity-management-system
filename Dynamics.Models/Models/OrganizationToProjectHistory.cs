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
        public int OrganizationResourceID { get; set; }
        public int? ProjectResourceID { get; set; }
        public int Status { get; set; }
        public string Unit { get; set; }
        public int Amount { get; set; }
        public string? Message { get; set; }
		[DataType(DataType.Date)]
		public DateOnly Time { get; set; }
        public virtual OrganizationResource OrganizationResource { get; set; }
		public virtual ProjectResource ProjectResource { get; set; }
	}
}
