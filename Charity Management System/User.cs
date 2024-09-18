namespace Charity_Management_System
{
    public class User
    {
        public int userID { get; set; }
        public string name { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
        public string password { get; set; }
        public int roleID { get; set; }
        public int? awardID { get; set; }
        public string avatar { get; set; }
        public string description { get; set; }
        public virtual ICollection<Award> award { get; set; }
        public virtual ICollection<ProjectMember> projectMember { get; set; }
        public virtual ICollection<OrganizationMember> organizationMember { get; set; }
        public virtual ICollection<Request> request { get; set; }
        public virtual ICollection<UserToOrganizationTransactionHistory> userToOrganizationTransactionHistory { get; set; }
        public virtual ICollection<UserToProjectTransactionHistory> userToProjectTransactionHistory { get; set; }
    }
}
