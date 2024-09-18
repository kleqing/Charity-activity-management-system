using System.ComponentModel.DataAnnotations;

namespace BussinessObject
{
    public class User
    {
		public int userID { get; set; }
        public string name { get; set; }
		public DateOnly? dob { get; set; }
		public string email { get; set; }
		public string phoneNumber { get; set; }
		public string? address { get; set; }
		[Required]
		public string password { get; set; }
		public int? roleID { get; set; }
		public string avatar { get; set; }
		public string description { get; set; }
		public virtual Request Request { get; set; }
		public virtual ICollection<ProjectMember> ProjectMember { get; set; }
		public virtual ICollection<OrganizationMember> OrganizationMember { get; set; }
		public virtual ICollection<UserToOrganizationTransactionHistory> UserToOrganizationTransactions { get; set; }
		public virtual ICollection<UserToProjectTransactionHistory> UserToProjectTransactions { get; set; }
	}
}
