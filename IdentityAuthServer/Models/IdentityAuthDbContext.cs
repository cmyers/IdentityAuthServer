using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityAuthServer.Models
{
    public class IdentityAuthDbContext : IdentityDbContext<AppUser>
    {
        public IdentityAuthDbContext(DbContextOptions<IdentityAuthDbContext> options)
              : base(options)
        {
        }

    }
}
