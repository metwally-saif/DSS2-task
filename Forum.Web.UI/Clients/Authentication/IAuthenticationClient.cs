using Forum.Web.UI.Clients.Users;
using Refit;

namespace Forum.Web.UI.Clients.Authentication
{
    public interface IAuthenticationClient
    {
        [Post("/")]
        Task<UserDetailsResponse> LoginAsync(
            AuthenticateDto authenticateDto);
    }
}