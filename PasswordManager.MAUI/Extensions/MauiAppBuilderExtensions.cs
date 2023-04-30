using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.ViewModels;
using PasswordManager.MAUI.Views;
using Persistance.MAUI.Repositories;
using Services;
using Services.Abstraction;
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
            // Singleton
            builder.Services.AddSingleton(Connectivity.Current);
            builder.Services.AddSingleton<HttpClient>();

            builder.Services.AddSingleton<IMauiAuthService, MauiAuthService>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<ITwoFactorAuthService, TwoFactorAuthService>();
            builder.Services.AddSingleton<IMauiSyncService, MauiSyncService>();
            builder.Services.AddSingleton<ISyncService, SyncService>();

            builder.Services.AddSingleton<IMauiPasswordService, MauiPasswordService>();

            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IPasswordService, PasswordService>();

            builder.Services.AddSingleton<IUserRepository, MauiUserRepository>();
            builder.Services.AddSingleton<IPasswordRepository, MauiPasswordRepository>();

            // Transient
            builder.Services.AddTransient<IJwtService, JwtService>();
            builder.Services.AddTransient<IEmailService, EmailService>();

            return builder;
        }

        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            // Singleton

            // Transient
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginTfaViewModel>();

            builder.Services.AddTransient<RegistrationViewModel>();
            builder.Services.AddTransient<RegistrationSuccessfulViewModel>();

            builder.Services.AddSingleton<AddEditPasswordViewModel>();
            builder.Services.AddTransient<PasswordsListViewModel>();
            builder.Services.AddTransient<GeneratePasswordViewModel>();

            builder.Services.AddTransient<TfaSettingsViewModel>();

            return builder;
        }

        public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
        {
            // Singleton

            // Transient
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginTfaPage>();

            builder.Services.AddTransient<RegistrationPage>();
            builder.Services.AddTransient<RegistrationSuccessfulPage>();

            builder.Services.AddTransient<AddEditPasswordPage>();
            builder.Services.AddTransient<PasswordsListPage>();
            builder.Services.AddTransient<GeneratePasswordPage>();

            builder.Services.AddTransient<TfaSettingsPage>();

            return builder;
        }

    }
}
