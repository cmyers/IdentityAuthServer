
using AuthServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Services
{
    public class DataService : IDataService
    {
        private readonly DataDbContext _dataContext;

        public DataService(DataDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _dataContext.Users.ToListAsync();
            return users;
        }

        public bool AddUser()
        {
            _dataContext.Users.Add(new User());
            return true;
        }
    }
}