using AuthServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthServer.Services
{
    public interface IUserService
    {
        Task<bool> Authenticate(Login login);
        Task<IEnumerable<User>> GetUsers();
    }
}