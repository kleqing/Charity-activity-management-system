using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Models
{
    public class ProjectResource
    {
		public Guid ResourceID { get; set; }
		public Guid ProjectID { get; set; }

        [Required(ErrorMessage = "The Resource Name field is required *")]
        public string ResourceName { get; set; }
		public int? Quantity { get; set; }
		public int? ExpectedQuantity { get; set; }

        [Required(ErrorMessage = "The Unit field is required *")]
        public string Unit { get; set; }
		public virtual Project Project { get; set; }
        public virtual ICollection<OrganizationToProjectHistory> OrganizationToProjectHistory { get; set; }
        public virtual ICollection<UserToProjectTransactionHistory> UserToProjectTransactionHistory { get; set; }


    }
}
