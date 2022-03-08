using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PasswordManager.Services;
using MAUIModelsLib;
using Command = MvvmHelpers.Commands.Command;
using MvvmHelpers;
using PasswordManager.Views;
using MvvmHelpers.Commands;
using Microsoft.Maui.Essentials;

namespace PasswordManager.ViewModels
{
    public class GeneratePasswordViewModel : BaseViewModel
    {
        public ICommand GenerateNewCommand { get; }
        public ICommand CopyCommand { get; }

        public GeneratePasswordViewModel()
        {
            Title = "Generate password";

            GenerateNewCommand = new AsyncCommand(GenerateNew);
            CopyCommand = new AsyncCommand(Copy);
        }

        string _password = "";
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private async Task Copy()
        {
            await Clipboard.SetTextAsync(Password);
        }

        public async Task GenerateNew()
        {

        }
    }
}
