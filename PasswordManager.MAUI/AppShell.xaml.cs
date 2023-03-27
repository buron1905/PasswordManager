using PasswordManager.MAUI.ViewModels;
using PasswordManager.MAUI.Views;

namespace PasswordManager.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            BindingContext = new AppShellViewModel();

            Routing.RegisterRoute(nameof(LoadingPage),
                    typeof(LoadingPage));

            Routing.RegisterRoute(nameof(LoginPage),
                    typeof(LoginPage));

            Routing.RegisterRoute(nameof(LoginTfaPage),
                    typeof(LoginTfaPage));

            Routing.RegisterRoute(nameof(RegistrationPage),
                    typeof(RegistrationPage));

            Routing.RegisterRoute(nameof(PasswordsListPage),
                    typeof(PasswordsListPage));

            Routing.RegisterRoute(nameof(GeneratePasswordPage),
                    typeof(GeneratePasswordPage));

            Routing.RegisterRoute(nameof(AddEditPasswordPage),
                    typeof(AddEditPasswordPage));

            Routing.RegisterRoute(nameof(SettingsPage),
                    typeof(SettingsPage));

            Routing.RegisterRoute(nameof(RegistrationSuccessfulPage),
                    typeof(RegistrationSuccessfulPage));

        }
    }
}