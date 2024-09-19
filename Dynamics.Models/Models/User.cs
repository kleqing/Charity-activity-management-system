using System.ComponentModel.DataAnnotations;

namespace Dynamics.Models.Models
{
    public class User
    {
        [Required]
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string? Address { get; set; }
        [Display(Name = "Phone Number")]
        [MaxLength(10, ErrorMessage = "Phone number must be 10 digits")]
        public string? Phone { get; set; }
        [Display(Name = "Date of Birth")]
        public DateTime Dob { get; set; }
        [Required]
        public int RoleId { get; set; }
        public string? Avatar { get; set; }

    }
}
