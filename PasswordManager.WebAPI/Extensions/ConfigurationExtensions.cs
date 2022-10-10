namespace PasswordManager.WebAPI.Extensions
{
    public static class ConfigurationExtensions
    {
        public static AppSettings GetAppSettings(this IConfiguration configuration, IServiceCollection services)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            return appSettingsSection.Get<AppSettings>();
        }
    }
}
