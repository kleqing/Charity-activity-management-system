using System.ComponentModel.DataAnnotations;

namespace Dynamics.Models.Models
{
    public class User
    {
        public Guid UserID { get; set; }
        public string UserFullName { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? UserDOB { get; set; }
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }
        [DataType(DataType.PhoneNumber)]        
        public string? UserPhoneNumber { get; set; }
        public string? UserAddress { get; set; }
        public string? UserAvatar { get; set; }
        public string? UserDescription { get; set; }
        public bool isBanned { get; set; }
        public virtual ICollection<Award> Award { get; set; }
        public virtual ICollection<Request> Request { get; set; }
        public virtual ICollection<ProjectMember> ProjectMember { get; set; }
        public virtual ICollection<OrganizationMember> OrganizationMember { get; set; }
        public virtual ICollection<UserToOrganizationHistory> UserToOrganizationTransactions { get; set; }
        public virtual ICollection<UserToProjectHistory> UserToProjectTransactions { get; set; }
    }
}
