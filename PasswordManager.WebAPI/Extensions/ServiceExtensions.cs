using EFDataAccessLib.Repositories;
using LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.WebAPI.Helpers;
using PasswordManager.WebAPI.Services;
using Services;
using Services.Abstraction;
using System.Text;

namespace PasswordManager.WebAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<IServiceWrapper, ServiceWrapper>();
            return services;
        }

        public static IServiceCollection ConfigureJwtAuthentication(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddAuthentication(opt => {
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
            services.AddAuthentication(opt => {
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
                    IssuerSigningKey = JwtMiddleware._publicSigningKey,
                    
                    // private key for encryption
                    TokenDecryptionKey = JwtMiddleware._privateEncryptionKey,

                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    //ClockSkew = TimeSpan.Zero
                };
            });
            return services;
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
    }
}
