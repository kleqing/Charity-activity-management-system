using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models
{
    public class UserToOrganizationTransactionHistory
    {
		public int TransactionID { get; set; }
        public int ResourceID { get; set; }
        public string UserID { get; set; }
		//public int OrganizationID { get; set; }
        public int Status { get; set; }
        public string Unit { get; set; }
        public int Amount { get; set; }
        public string? Message { get; set; }
		[DataType(DataType.Date)]
		public DateOnly Time { get; set; }
        public virtual User User { get; set; }
		public virtual OrganizationResource OrganizationResource { get; set; }
	}
}
