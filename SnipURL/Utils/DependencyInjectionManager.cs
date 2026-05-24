using Microsoft.Extensions.DependencyInjection;
using SnipURL.Repositories;
using SnipURL.Services;
using SnipURL.Utils;

namespace SnipURL.Configuration
{
    /// <summary>
    /// Static configuration manager to handle dependency injection registrations 
    /// for the SnipURL application infrastructure.
    /// </summary>
    public static class DependencyInjectionManager
    {
        #region METHODS
        /// <summary>
        /// Registers all application singletons and core services into the DI container.
        /// </summary>
        /// <param name="services">The application service collection being extended.</param>
        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<FileStorageUtil>();
            services.AddSingleton<RepositorySerializer>();
            services.AddSingleton<ShortenedUrlRepository>();

            services.AddScoped<ShortUrlService>();
            services.AddHostedService<StartUpService>();
        }
        #endregion
    }
}