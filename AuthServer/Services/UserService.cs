using AuthServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Services
{
    public class UserService : IUserService
    {
        private readonly DataDbContext _dataContext;

        public UserService(DataDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _dataContext.Users.ToListAsync();
            return users;
        }

        public async Task<bool> Authenticate(Login login)
        {
            var user = await _dataContext.Users.Where(u => u.Username == login.UserName && u.Password == login.Password).FirstOrDefaultAsync();
            if (user != null)
            {
                return true;
            }
            return false;
        }

        public bool AddUser()
        {
            _dataContext.Users.Add(new User());
            return true;
        }
    }
}