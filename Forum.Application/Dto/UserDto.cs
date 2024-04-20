using Forum.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Forum.Application.Dto
{
    public class CreateUserDto
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? ConfirmPassword { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
