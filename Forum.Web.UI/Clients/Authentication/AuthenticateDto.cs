using System.ComponentModel.DataAnnotations;

namespace Forum.Web.UI.Clients.Authentication
{
    public class AuthenticateDto
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}