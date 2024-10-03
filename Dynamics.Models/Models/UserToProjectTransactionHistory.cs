using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models
{
    public class UserToProjectTransactionHistory
    {
		public Guid TransactionID { get; set; }
		public Guid UserID { get; set; }
		public Guid ProjectID { get; set; }
        [Required]
        public Guid? ResourceID  { get; set; }
        public int Status { get; set; }
        //change to double, delete MoneyTransactionAmout
        [Required]
        public int Amount { get; set; }
        public string? Message { get; set; }
		public DateTime? Time { get; set; }
        public virtual ProjectResource? Resource { get; set; }
        public virtual User User { get; set; }
		public virtual Project Project { get; set; }

	}
}
