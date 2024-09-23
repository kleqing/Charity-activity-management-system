using System.ComponentModel.DataAnnotations;

namespace Dynamics.Models.Models
{
    public class User
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Dob { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
        [MaxLength(300)]
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        [MaxLength(300)]
        public string? Description { get; set; }
        public bool IsBanned { get; set; }
        // Role ID and password will be passed to identity table instead
    }
}
