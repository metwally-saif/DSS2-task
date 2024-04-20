using System.ComponentModel.DataAnnotations;

namespace Forum.Application.Dto
{
    public class AuthenticateDto
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}