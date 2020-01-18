namespace IdentityAuthServer.Models
{
    public interface IUserDetails
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
    }
}
