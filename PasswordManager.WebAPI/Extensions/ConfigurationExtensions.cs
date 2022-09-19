namespace PasswordManager.WebAPI.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetDefaultConnectionString(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("DefaultConnection");
        }

        public static AppSettings GetAppSettings(this IConfiguration configuration, IServiceCollection services)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            return appSettingsSection.Get<AppSettings>();
        }
    }
}
