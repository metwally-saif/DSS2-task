using Forum.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.Application
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection AddApplicationLayer(
            this IServiceCollection services)
        {
            services.AddScoped<UserService>();
            services.AddScoped<AuthenticationService>();
            _ = services.AddSingleton<PasswordService>();

            return services;
        }
    }
}
