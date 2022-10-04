using LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.WebAPI.Services;
using Services;
using Services.Abstraction;
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

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPasswordService, PasswordService>();
            return services;

            //var assembly = Assembly.GetExecutingAssembly();
            //var types = assembly.GetTypes().Where(t => t.IsClass && (t.Namespace?.Contains("Services") ?? false));
            //foreach (var type in types)
            //{
            //    var interfaces = type.GetInterfaces();
            //    foreach (var @interface in interfaces)
            //    {
            //        services.AddTransient(@interface, type);
            //    }
            //}
            //return services;
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

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
