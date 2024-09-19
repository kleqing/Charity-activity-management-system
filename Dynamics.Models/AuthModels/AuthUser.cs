using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Dynamics.Models.AuthModels
{
    // custom our identity user
    public class AuthUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public int roleId { get; set; }
    }
}
