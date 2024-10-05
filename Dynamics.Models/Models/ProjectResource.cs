using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Dynamics.Models.Models
{
    public class ProjectResource
    {
		public Guid ResourceID { get; set; }
		public Guid ProjectID { get; set; }

        [Required(ErrorMessage = "The Resource Name field is required *")]
        public string ResourceName { get; set; }
        [ValidateNever]
        public int? Quantity { get; set; }
        [Required]
        public int ExpectedQuantity { get; set; }

        [Required(ErrorMessage = "The Unit field is required *")]
        
        public string Unit { get; set; }
        [ValidateNever]
        public virtual Project Project { get; set; }
        [ValidateNever]
        public virtual ICollection<OrganizationToProjectHistory> OrganizationToProjectHistory { get; set; }
        [ValidateNever]
        public virtual ICollection<UserToProjectTransactionHistory> UserToProjectTransactionHistory { get; set; }


    }
}
