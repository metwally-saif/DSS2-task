using System.ComponentModel.DataAnnotations;

namespace Forum.Application.Dto
{
    public class UpdateUserDto
    {

        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Email { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
