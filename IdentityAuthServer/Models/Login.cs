using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IdentityAuthServer.Models
{
    public class Login
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [JsonPropertyName("grant_type")]
        [Required]
        public string GrantType { get; set; }

    }
}