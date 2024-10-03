using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models.ViewModel
{
    public class SendDonateRequestVM
    {
        public Guid ProjectID { get; set; }
        public string TypeDonor { get; set; }
        public UserToProjectTransactionHistory? UserDonate { get; set; }
        public OrganizationToProjectHistory? OrgDonate { get; set; }
        public List<UserToProjectTransactionHistory>? UserTransactionHistory { get; set; }
        public List<OrganizationToProjectHistory>? OrgTransactionHistory { get; set; }
    }
}
