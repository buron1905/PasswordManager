using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Models;
using Persistance.Repositories;
using Services;
using Services.Abstraction;
using Services.Abstraction.Auth;
using Services.Abstraction.Data;
using Services.Abstraction.Data.Persistance;
using Services.Auth;
using Services.Data;
using Services.TMP;
using System.Text;

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
            services.AddTransient<ISettingsService, SettingsService>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPasswordRepository, PasswordRepository>();
            services.AddTransient<ISettingsRepository, SettingsRepository>();

            return services;
        }

        public static IServiceCollection ConfigureJwtAuthentication(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "https://localhost:5001",
                    ValidAudience = "https://localhost:5001",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Secret))
                };
            });
            return services;
        }

        public static IServiceCollection ConfigureJweAuthentication(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    //ValidIssuer = "https://localhost:5001",
                    //ValidAudience = "https://localhost:5001",

                    //ValidateIssuer = true,
                    //ValidateAudience = true,
                    //ValidateLifetime = true,

                    // public key for signing
                    IssuerSigningKey = JWTKeys._publicSigningKey,

                    // private key for encryption
                    TokenDecryptionKey = JWTKeys._privateEncryptionKey,

                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    //ClockSkew = TimeSpan.Zero
                };
            });
            return services;
        }

        //public static void ConfigureLoggerService(this IServiceCollection services)
        //{
        //    services.AddSingleton<ILoggerManager, LoggerManager>();
        //}
    }
}
