using System;
using System.Collections.Generic;

namespace AuthServer.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? Created { get; set; }
    }
}
