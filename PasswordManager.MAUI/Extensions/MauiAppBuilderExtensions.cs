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
            builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
            builder.Services.AddSingleton<HttpClient>();

            //Singleton
            //// From WebAPI
            builder.Services.AddSingleton<IRepositoryWrapper, RepositoryWrapper>();
            builder.Services.AddSingleton<IDataServiceWrapper, DataServiceWrapper>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<ITwoFactorAuthService, TwoFactorAuthService>();
            builder.Services.AddSingleton<MauiAuthService>();
            builder.Services.AddSingleton<ISyncService, MauiSyncService>();

            // Transient
            builder.Services.AddTransient<IJwtService, JwtService>();
            builder.Services.AddTransient<IEmailService, EmailService>();

            return builder;
        }

        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            //Singleton
            builder.Services.AddTransient<LoadingViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginTfaViewModel>();
            builder.Services.AddTransient<RegistrationViewModel>();
            builder.Services.AddTransient<RegistrationSuccessfulViewModel>();
            builder.Services.AddTransient<PasswordsListViewModel>();
            builder.Services.AddTransient<GeneratePasswordViewModel>();
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<TfaSettingsViewModel>();

            // Transient
            builder.Services.AddTransient<AddEditPasswordViewModel>();

            return builder;
        }

        public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
        {
            //Singleton
            builder.Services.AddTransient<LoadingPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginTfaPage>();
            builder.Services.AddTransient<RegistrationPage>();
            builder.Services.AddTransient<RegistrationSuccessfulPage>();
            builder.Services.AddTransient<PasswordsListPage>();
            builder.Services.AddTransient<GeneratePasswordPage>();
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<TfaSettingsPage>();

            // Transient
            builder.Services.AddTransient<AddEditPasswordPage>();

            return builder;
        }

    }
}
