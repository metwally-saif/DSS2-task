using Forum.Web.UI.Clients.Users;

namespace Forum.Web.UI.Models
{
    public class CreateUserViewModel
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Role? Role { get; set; }
    }
}
