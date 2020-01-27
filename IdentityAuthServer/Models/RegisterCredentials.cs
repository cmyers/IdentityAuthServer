using System.ComponentModel.DataAnnotations;

namespace IdentityAuthServer.Models
{
    public class RegisterCredentials : UserDetails, IUserDetails
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
