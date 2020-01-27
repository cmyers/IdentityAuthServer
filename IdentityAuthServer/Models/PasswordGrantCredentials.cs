using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IdentityAuthServer.Models
{
    public class PasswordGrantCredentials
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [JsonPropertyName("grant_type")]
        [Required]
        public string GrantType { get; set; }

        [JsonPropertyName("client_id")]
        [Required]
        public string ClientId { get; set; }

        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }

        public string Scope { get; set; }


    }
}