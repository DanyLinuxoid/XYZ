using XYZ.Models.System.Configuration;

namespace XYZ.Web
{
    public static class ServiceConfiguration
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            // Config
            var configRaw = configuration.GetSection("AppConfiguration");
            var config = configRaw.Get<AppConfiguration>();
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            else if (string.IsNullOrEmpty(config.ConnectionString))
                throw new ArgumentNullException(nameof(config.ConnectionString));

            services.Configure<AppConfiguration>(configRaw);

            // DI
            DependencyInjector.RegisterClasses(services, configuration);
        }
    }
}
