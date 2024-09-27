using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models
{
    public class UserToProjectHistory
    {
		public Guid TransactionID { get; set; }
		public Guid UserID { get; set; }
		public Guid ProjectID { get; set; }
		public int Status { get; set; }
		public string Unit { get; set; }
		public int Amount { get; set; }
        public string Message { get; set; }
		public string Time { get; set; }
        public virtual User User { get; set; }
		public virtual Project Project { get; set; }
	}
}
