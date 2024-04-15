using Forum.Application.Repositories;
using Forum.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Forum.Infrastructure
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var section = configuration.GetSection(nameof(DatabaseSettings));
            services.Configure<DatabaseSettings>(section);

            services.AddDbContext<DatabaseContext>((provider, options) =>
            {
                var settings = provider.GetRequiredService<IOptions<DatabaseSettings>>();
                options.UseSqlite(settings.Value.ConnectionString, config =>
                {
                    config.MigrationsHistoryTable(settings.Value.MigrationTable);

                    config.MigrationsAssembly(typeof(DatabaseContext)
                        .Assembly.GetName()
                        .Name);
                });
            });

            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public static IHost UseMigrations(this IHost app)
        {
            var settings = app.Services.GetRequiredService<IOptions<DatabaseSettings>>();

            if (settings.Value.EnableMigrations)
            {
                using (var scope = app.Services.CreateScope())
                {
                    var context = scope.ServiceProvider
                        .GetRequiredService<DatabaseContext>();

                    context.Database.Migrate();
                }
            }

            return app;
        }
    }
}
