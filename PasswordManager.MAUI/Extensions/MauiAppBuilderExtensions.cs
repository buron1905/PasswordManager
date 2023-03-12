using PasswordManager.MAUI.ViewModels;
using PasswordManager.MAUI.Views;

namespace PasswordManager.MAUI.Extensions
{
    public static class MauiAppBuilderExtensions
    {
        public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);

            //Singleton
            //// From WebAPI
            //builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            //builder.Services.AddScoped<IDataServiceWrapper, DataServiceWrapper>();

            // Transient
            //builder.Services.AddTransient<IJwtService, JwtService>();
            //builder.Services.AddTransient<IAuthService, AuthService>();
            //builder.Services.AddTransient<IPasswordService, PasswordService>();

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

            // Transient

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

            // Transient

            return builder;
        }
    }
}
