using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Models
{
    public class AppUser : IdentityUser
    {
        // custom properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
