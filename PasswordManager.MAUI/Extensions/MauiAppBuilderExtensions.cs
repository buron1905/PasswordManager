using Microsoft.EntityFrameworkCore;
using PasswordManager.MAUI.Helpers;
using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.ViewModels;
using PasswordManager.MAUI.Views;
using Persistance;
using Persistance.Repositories;
using Services.Abstraction.Auth;
using Services.Abstraction.Data;
using Services.Abstraction.Data.Persistance;
using Services.Auth;
using Services.Data;

namespace PasswordManager.MAUI.Extensions
{
    public static class MauiAppBuilderExtensions
    {
        public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
            builder.Services.AddSingleton<HttpClient>();

            //Singleton
            //// From WebAPI
            builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            builder.Services.AddScoped<IDataServiceWrapper, DataServiceWrapper>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<MauiAuthService>();

            // Transient
            builder.Services.AddTransient<IJwtService, JwtService>();
            builder.Services.AddTransient<IPasswordService, PasswordService>();

            return builder;
        }

        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            //Singleton
            builder.Services.AddSingleton<LoadingViewModel>();
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<RegistrationViewModel>();
            builder.Services.AddSingleton<PasswordsListViewModel>();
            builder.Services.AddSingleton<GeneratePasswordViewModel>();
            builder.Services.AddSingleton<SettingsViewModel>();

            // Transient
            builder.Services.AddTransient<AddEditPasswordViewModel>();

            return builder;
        }

        public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
        {
            //Singleton
            builder.Services.AddSingleton<LoadingPage>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<RegistrationPage>();
            builder.Services.AddSingleton<PasswordsListPage>();
            builder.Services.AddSingleton<GeneratePasswordPage>();
            builder.Services.AddSingleton<SettingsPage>();

            // Transient
            builder.Services.AddTransient<AddEditPasswordPage>();

            return builder;
        }

        public static MauiAppBuilder AddDbContext(this MauiAppBuilder builder)
        {
            builder.Services.AddDbContext<DataContext>(opt =>
                opt.UseSqlite($"Data Source={Constants.DatabaseFilename}"));
            //opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
            return builder;
        }

    }
}
