using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PasswordManager.Services;

namespace PasswordManager.ViewModels
{
    public class PasswordsListViewModel : BindableObject
    {
        public Action DisplayInvalidLoginPrompt;

        public PasswordsListViewModel()
        {
            Logout = new Command(OnLogout);
        }
        
        public ICommand Logout { get; }

        private async void OnLogout()
        {
            ActiveUserService.Instance.Logout();

            (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.InsertPageBefore(new Views.LoginPage(), Application.Current.MainPage.Navigation.NavigationStack[0]);
            await (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.PopToRootAsync();
        }
    }
}
