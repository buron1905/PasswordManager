using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services
{
    public class PopupService
    {

        public static void ShowError()
        {
            Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Error", "Unexpected error happened", "OK");
        }

        public static void ShowError(string title, string message, string cancel = "OK")
        {
            Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
    }
}
