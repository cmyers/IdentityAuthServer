using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Models
{
    public class AuthIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AuthIdentityDbContext(DbContextOptions<AuthIdentityDbContext> options)
              : base(options)
        {
        }

    }
}
