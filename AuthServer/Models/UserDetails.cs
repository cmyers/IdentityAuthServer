using System.ComponentModel.DataAnnotations;

namespace AuthServer.Models
{
    public class UserDetails: IUserDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

    }
}
