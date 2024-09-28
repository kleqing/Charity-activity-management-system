using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models
{
    public class UserToProjectTransactionHistory
    {
		public string ResourceName { get; set; }
		public int TransactionID { get; set; }
		public string UserID { get; set; }
		public int ProjectID { get; set; }
		public int Status { get; set; }
		public int Unit { get; set; }
		public int Amount { get; set; }
        public string Message { get; set; }
		public string Time { get; set; }
        public string MoneyTransactionAmout { get; set; }
        public virtual User User { get; set; }
		public virtual Project Project { get; set; }
	}
}
