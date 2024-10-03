using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models.ViewModel
{
    public class ProjectTransactionHistoryVM
    {
        public List<UserToProjectTransactionHistory> UserDonate { get; set; }
        public List<OrganizationToProjectHistory> OrganizationDonate { get; set; }
    }
}
