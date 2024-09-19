using System.ComponentModel.DataAnnotations;

namespace Dynamics.Models.Models
{
    public class User
    {
        
        public int userID { get; set; }
        public string name { get; set; }
        [DataType(DataType.Date)]
        public DateTime? dob { get; set; }
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? phoneNumber { get; set; }
        public string? address { get; set; }
        //[Required]
        //[DataType(DataType.Password)]
        //public string password { get; set; }
        public int? roleID { get; set; }
        public string? avatar { get; set; }
        public string? description { get; set; }
    }
}
