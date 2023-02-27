namespace PasswordManager.MAUI.Services
{
    public class PopupService
    {

        public static async Task ShowError()
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Unexpected error happened", "OK");
        }

        public static async Task ShowError(string title, string message, string cancel = "OK")
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public static async Task<bool> ShowYesNo(string title, string question, string optionTrue = "Yes", string optionFalse = "No")
        {
            return await Application.Current.MainPage.DisplayAlert(title, question, optionTrue, optionFalse);
        }

        public static async Task<string> ShowActionSheet(string title, string cancel, string destruction, params string[] buttons)
        {
            return await Application.Current.MainPage.DisplayActionSheet(title, cancel, destruction, buttons);
        }

        public static async Task<string> ShowPrompt(string title, string message, string accept = "OK", string cancel = "Cancel", string placeholder = null,
            int maxLength = -1, Keyboard keyboard = null, string initialValue = "")
        {
            return await Application.Current.MainPage.DisplayPromptAsync(title, message, accept, cancel, placeholder, maxLength, keyboard, initialValue);
            //return await Application.Current.MainPage.DisplayPromptAsync("Question 2", "What's 5 + 5?", initialValue: "10", maxLength: 2, keyboard: Keyboard.Numeric);
        }
    }
}
