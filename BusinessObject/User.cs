using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace BussinessObject
{
    public class User
    {
		public int userID { get; set; }
        public string userName { get; set; }
		[DataType(DataType.Date)]
		public DateOnly? userDOB { get; set; }
		public string userEmail { get; set; }
		public string userPhoneNumber { get; set; }
		public string? userAddress { get; set; }
		[Required]
		public string userPassword { get; set; }
		public int? userRoleID { get; set; }
		public string? userAvatarURL { get; set; }
		[NotMapped]
		public IFormFile? userAvatar { get; set; } = null;
		public string userDescription { get; set; }
		public virtual Request Request { get; set; }
		public virtual ICollection<ProjectMember> ProjectMember { get; set; }
		public virtual ICollection<OrganizationMember> OrganizationMember { get; set; }
		public virtual ICollection<UserToOrganizationTransactionHistory> UserToOrganizationTransactions { get; set; }
		public virtual ICollection<UserToProjectTransactionHistory> UserToProjectTransactions { get; set; }
	}
}
