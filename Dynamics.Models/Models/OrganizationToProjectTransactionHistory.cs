using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models
{
    public class OrganizationToProjectTransactionHistory
    {
		public int transactionID { get; set; }
		public int organizationID { get; set; }
		public int projectID { get; set; }
		public string message { get; set; }
		public string time { get; set; }
		public string moneyTransactionAmout { get; set; }
        public virtual Organization Organization { get; set; }
		public virtual Project Project { get; set; }
	}
}
