using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.WebAPI.Features.Identity.Services;
using PasswordManager.WebAPI.Features.Passwords.Services;
using System.Text;

namespace PasswordManager.WebAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
            return services;
        }

        public static IServiceCollection ConfigureJwtAuthentication(this IServiceCollection services, AppSettings appSettings)
        {
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });
            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // AddScoped, AddSingleton, AddTransient
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IPasswordService, PasswordService>();
            return services;

            // add application services with reflection
            // var assembly = Assembly.GetExecutingAssembly();
            // var types = assembly.GetTypes().Where(t => t.IsClass && t.Namespace.Contains("Services"));
            // foreach (var type in types)
            
        }

        //public static IServiceCollection ConfigureIISIntegration(this IServiceCollection services)
        //{
        //    services.Configure<IISOptions>(options =>
        //    {
        //    });
        //    return services;
        //}
        //public static void ConfigureLoggerService(this IServiceCollection services)
        //{
        //    services.AddSingleton<ILoggerManager, LoggerManager>();
        //}
        //public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration config)
        //{
        //    var connectionString = config["mysqlconnection:connectionString"];
        //    services.AddDbContext<RepositoryContext>(o => o.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
        //}
        //public static void ConfigureRepositoryManager(this IServiceCollection services)
        //{
        //    services.AddScoped<IRepositoryManager, RepositoryManager>();
        //}
    }
}
