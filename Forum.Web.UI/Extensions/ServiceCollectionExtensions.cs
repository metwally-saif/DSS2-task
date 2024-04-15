using Forum.Web.UI.Clients.Authentication;
using Forum.Web.UI.Clients.Users;
using Forum.Web.UI.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Forum.Web.UI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string ChallangeUrlPath = "/Home/Index";

        public static IServiceCollection RegisterBackendServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<BackendApi>(
               configuration.GetSection(nameof(BackendApi)));

            services.RegisterClient<IUserClient>("/api/users");
            services.RegisterClient<IAuthenticationClient>("/api/authentication");

            return services;
        }

        public static IServiceCollection AddCookieAuthentication(
            this IServiceCollection services,
            TimeSpan? expirationTime = null)
        {
            services
               .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(setting =>
               {
                   setting.AccessDeniedPath = ChallangeUrlPath;
                   setting.LoginPath = ChallangeUrlPath;
                   setting.Cookie.IsEssential = true;
                   setting.Cookie.HttpOnly = true;
                   setting.SlidingExpiration = true;
                   setting.ExpireTimeSpan = expirationTime ?? TimeSpan.FromMinutes(10);
               });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            return services;
        }
    }
}