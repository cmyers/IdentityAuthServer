using System.ComponentModel.DataAnnotations;

namespace IdentityAuthServer.Models
{
    public class Register : Login, IUserDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
