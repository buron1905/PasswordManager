using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace PasswordManager.Services
{
    public class PopupService
    {

        public static async void ShowError()
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Unexpected error happened", "OK");
        }

        public static async void ShowError(string title, string message, string cancel = "OK")
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public static async Task<bool> ShowYesNo(string title, string question, string optionTrue = "Yes", string optionFalse = "No")
        {
            return await Application.Current.MainPage.DisplayAlert(title, question, optionTrue, optionFalse);
        }

        //public static async Task<string> ShowActionSheet()
        //{
        //    return await Application.Current.MainPage.DisplayActionSheet("ActionSheet: Send to?", "Cancel", null, "Email", "Twitter", "Facebook");
        //}

        //public static async Task<string> ShowPrompt()
        //{
        //    //return await Application.Current.MainPage.DisplayPromptAsync("Question 1", "What's your name?");
        //    //return await Application.Current.MainPage.DisplayPromptAsync("Question 2", "What's 5 + 5?", initialValue: "10", maxLength: 2, keyboard: Keyboard.Numeric);
        //}
    }
}
