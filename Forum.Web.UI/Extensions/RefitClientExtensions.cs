using Forum.Web.UI.Settings;
using Microsoft.Extensions.Options;
using Refit;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace Forum.Web.UI.Extensions
{
    public static class RefitClientExtensions
    {
        public static IServiceCollection RegisterClient<T>(
            this IServiceCollection services,
            string controllerRoute)
            where T : class
        {
            services.AddRefitClient<T>()
               .ConfigureHttpClient((provider, client) =>
               {
                   var settings = provider
                       .GetRequiredService<IOptions<BackendApi>>();
                   client.BaseAddress = settings.Value.CreateUri(controllerRoute);
                   client.DefaultRequestHeaders.Accept.Clear();
                   client.DefaultRequestHeaders.Accept.Add(
                       new MediaTypeWithQualityHeaderValue(
                           MediaTypeNames.Application.Json));
               });

            return services;
        }
    }
}