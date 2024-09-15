using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charity_Management_System
{
    public class UserToProjectTransactionHistory
    {
        public int transactionID { get; set; }
        public int userID { get; set; }
        public int projectID { get; set; }
        public string message { get; set; }
        public DateTime time { get; set; }
        public virtual User user { get; set; }
        public virtual Project project { get; set; }
    }
}
