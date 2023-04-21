using CommunityToolkit.Maui.Alerts;

namespace PasswordManager.MAUI.Services
{
    public class AlertService
    {

        public static async Task ShowAlert(string title, string message, string cancel = "OK")
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public static async Task<bool> ShowYesNo(string title, string question, string optionTrue = "Yes", string optionFalse = "No")
        {
            return await Application.Current.MainPage.DisplayAlert(title, question, optionTrue, optionFalse);
        }

        public static async Task ShowToast(string text, CommunityToolkit.Maui.Core.ToastDuration duration = CommunityToolkit.Maui.Core.ToastDuration.Short)
        {
            await Toast.Make(text, duration).Show();
        }

        public static async Task ShowSnackbar(string text, IView? anchor = null)
        {
            await Snackbar.Make(text, anchor: anchor).Show();
        }

    }
}
