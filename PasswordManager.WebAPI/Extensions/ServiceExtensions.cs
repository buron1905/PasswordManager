using Persistance.Repositories;
using Services;
using Services.Abstraction;
using Services.Abstraction.Auth;
using Services.Abstraction.Data;
using Services.Abstraction.Data.Persistance;
using Services.Auth;
using Services.Data;

namespace PasswordManager.WebAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<IEmailService, EmailService>();

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ITwoFactorAuthService, TwoFactorAuthService>();

            services.AddTransient<ISyncService, SyncService>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPasswordService, PasswordService>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPasswordRepository, PasswordRepository>();

            return services;
        }
    }
}
