using Microsoft.Maui.Controls;
using PasswordManager.Views;

namespace PasswordManager;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(LoginPage),
                typeof(LoginPage));

        Routing.RegisterRoute(nameof(RegistrationPage),
				typeof(RegistrationPage));

        Routing.RegisterRoute(nameof(SettingsPage),
                typeof(SettingsPage));

        Routing.RegisterRoute(nameof(GeneratePasswordPage),
                typeof(GeneratePasswordPage));

        Routing.RegisterRoute(nameof(PasswordsListPage),
                typeof(PasswordsListPage));

        Routing.RegisterRoute(nameof(NewPasswordPage),
                typeof(NewPasswordPage));

        Routing.RegisterRoute(nameof(EditPasswordPage),
                typeof(EditPasswordPage));

        Routing.RegisterRoute(nameof(PasswordDetailPage),
                typeof(PasswordDetailPage));
    }
}