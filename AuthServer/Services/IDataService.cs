using AuthServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthServer.Services
{
    public interface IDataService
    {
        Task<IEnumerable<User>> GetUsers();
    }
}
