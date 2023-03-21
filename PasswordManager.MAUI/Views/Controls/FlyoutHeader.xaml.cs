using PasswordManager.MAUI.Services;

namespace PasswordManager.MAUI.Views.Controls;

public partial class FlyoutHeader : ContentView
{
    public FlyoutHeader()
    {
        InitializeComponent();

        lblUserName.Text = "Password Manager";

        if (ActiveUserService.Instance.IsActive && ActiveUserService.Instance.UserDTO.Email is not null)
        {
            lblUserEmail.IsVisible = true;
            lblUserEmail.Text = ActiveUserService.Instance.UserDTO.Email;
        }
        else
        {
            lblUserEmail.IsVisible = false;
            lblUserEmail.Text = string.Empty;
        }
    }
}