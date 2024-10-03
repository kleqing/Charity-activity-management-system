using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
		[Required]
		public string ResourceName { get; set; }
        [ValidateNever]
        public int? Quantity { get; set; }
		[Required]
		public int? ExpectedQuantity { get; set; }
		[Required]
		public string Unit { get; set; }
        [ValidateNever]
        public virtual Project Project { get; set; }
		[ValidateNever]
		public virtual ICollection<UserToProjectTransactionHistory>? UserToProjectTransactionHistories { get; set; }
		[ValidateNever]
		public virtual ICollection<OrganizationToProjectHistory>? OrganizationToProjectHistories { get; set; } 

    }
}
