using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Dynamics.Models.Models
{
    public class User
    {
        
        public Guid UserID { get; set; }
        [ValidateNever]
        public string UserFullName { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? UserDOB { get; set; }
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }
        [DataType(DataType.PhoneNumber)]        
        public string? UserPhoneNumber { get; set; }
        [ValidateNever]
        public string? UserAddress { get; set; }
        [ValidateNever]
        public string? UserAvatar { get; set; }
        public string? UserDescription { get; set; }
        [ValidateNever]
        // Self-referencing relationships for reports
        public virtual ICollection<Report> ReportsMade { get; set; }  // Reports this user submitted

        public virtual ICollection<Award> Award { get; set; }
        public virtual ICollection<Request> Request { get; set; }
        public virtual ICollection<ProjectMember> ProjectMember { get; set; }
        public virtual ICollection<OrganizationMember> OrganizationMember { get; set; }
        public virtual ICollection<UserToOrganizationTransactionHistory> UserToOrganizationTransactions { get; set; }
        public virtual ICollection<UserToProjectTransactionHistory> UserToProjectTransactions { get; set; }
    }
}
