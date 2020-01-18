using System.ComponentModel.DataAnnotations;

namespace IdentityAuthServer.Models
{
    public class UserDetails: IUserDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

    }
}
