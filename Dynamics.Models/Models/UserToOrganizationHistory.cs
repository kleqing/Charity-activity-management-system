﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models
{
    public class UserToOrganizationHistory
    {
		public Guid TransactionID { get; set; }
		public Guid UserID { get; set; }
		public Guid OrganizationID { get; set; }
		public string Message { get; set; }
		[DataType(DataType.Date)]
		public DateOnly Time { get; set; }
        public string MoneyTransactionAmout { get; set; }
        public virtual User User { get; set; }
		public virtual Organization Organization { get; set; }
	}
}
