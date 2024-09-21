using BussinessObject;
using System.ComponentModel.DataAnnotations;

namespace Dynamics.Models.Models
{
    public class User
    {
        
        public int userID { get; set; }
        public string userName { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? userDOB { get; set; }
        [DataType(DataType.EmailAddress)]
        public string userEmail { get; set; }
        [DataType(DataType.PhoneNumber)]        
        public string? userPhoneNumber { get; set; }
        public string? userAddress { get; set; }
        [Required]
        public string userPassword { get; set; }
        public int? userRoleID { get; set; }
        public string? userAvatar { get; set; }
        public string? userDescription { get; set; }
        public virtual ICollection<Award> Award { get; set; }
        public virtual ICollection<Request> Request { get; set; }
        public virtual ICollection<ProjectMember> ProjectMember { get; set; }
        public virtual ICollection<OrganizationMember> OrganizationMember { get; set; }
        public virtual ICollection<UserToOrganizationTransactionHistory> UserToOrganizationTransactions { get; set; }
        public virtual ICollection<UserToProjectTransactionHistory> UserToProjectTransactions { get; set; }
    }
}
