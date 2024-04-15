namespace Forum.Web.UI.Clients.Users
{
    public class UserDetailsResponse
    {
        public long? Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Role? Role { get; set; }
    }
}
