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

            // Transient
            builder.Services.AddTransient<IMauiAuthService, MauiAuthService>();
            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddTransient<ITwoFactorAuthService, TwoFactorAuthService>();
            builder.Services.AddTransient<IMauiSyncService, MauiSyncService>();
            builder.Services.AddTransient<ISyncService, SyncService>();

            builder.Services.AddTransient<IMauiPasswordService, MauiPasswordService>();

            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IPasswordService, PasswordService>();

            builder.Services.AddTransient<IUserRepository, MauiUserRepository>();
            builder.Services.AddTransient<IPasswordRepository, MauiPasswordRepository>();

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

            builder.Services.AddSingleton<AddEditPasswordPage>();
            builder.Services.AddTransient<PasswordsListPage>();
            builder.Services.AddTransient<GeneratePasswordPage>();

            builder.Services.AddTransient<TfaSettingsPage>();
            builder.Services.AddTransient<AboutPage>();

            return builder;
        }

    }
}
