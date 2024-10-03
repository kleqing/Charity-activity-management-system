using System.ComponentModel.DataAnnotations;

namespace Dynamics.Models.Models
{
    public class Organization
    {
        [Required]
        public Guid OrganizationID { get; set; }
        [Required]
        public string OrganizationName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? OrganizationEmail { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? OrganizationPhoneNumber { get; set; }
        public string? OrganizationAddress { get; set; }
        public string OrganizationDescription { get; set; }
        public string? OrganizationPictures { get; set; }
        [DataType(DataType.Date)]
        public DateOnly StartTime { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? ShutdownDay { get; set; }
        public Guid? CEOID { get; set; }

        public virtual ICollection<Project> Project { get; set; }
        public virtual ICollection<OrganizationMember> OrganizationMember { get; set; }
        public virtual ICollection<OrganizationResource> OrganizationResource { get; set; }
    }
}
