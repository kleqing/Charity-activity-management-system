using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject
{
    public class UserToOrganizationTransactionHistory
    {
		public int transactionID { get; set; }
		public int userID { get; set; }
		public int organizationID { get; set; }
		public string message { get; set; }
		public string time { get; set; }
		public virtual User User { get; set; }
		public virtual Organization Organization { get; set; }
	}
}
