using Models;

namespace PasswordManager.WebAPI.Extensions
{
    public static class ConfigurationExtensions
    {
        public static AppSettings? GetAppSettings(this IConfiguration configuration, IServiceCollection services)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            return appSettingsSection.Get<AppSettings>();
        }

        public static EmailConfiguration? GetEmailSettings(this IConfiguration configuration, IServiceCollection services)
        {
            var emailSettingsSection = configuration.GetSection("EmailConfiguration");
            services.Configure<EmailConfiguration>(emailSettingsSection);
            return emailSettingsSection.Get<EmailConfiguration>();
        }

    }
}
