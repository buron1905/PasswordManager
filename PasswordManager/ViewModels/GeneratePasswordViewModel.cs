using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PasswordManager.Services;
using Models;
using Command = MvvmHelpers.Commands.Command;
using MvvmHelpers;
using PasswordManager.Views;
using MvvmHelpers.Commands;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel.DataTransfer;

namespace PasswordManager.ViewModels
{
    public class GeneratePasswordViewModel : BaseViewModel
    {
        public ICommand GenerateNewCommand { get; }
        public ICommand CopyCommand { get; }

        public GeneratePasswordViewModel()
        {
            Title = "Generate password";

            GenerateNewCommand = new Command(GenerateNew);
            CopyCommand = new AsyncCommand(Copy);
        }

        string _password = "";
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        int _length;
        public int Length
        {
            get => Preferences.Get(nameof(Length), 8);
            set
            {
                SetProperty(ref _length, value);
                Preferences.Set(nameof(Length), value);
                LengthText = $"Length {_length}";
                GenerateNew();
            }
        }

        string _lengthText = "";
        public string LengthText
        {
            get => _lengthText;
            set => SetProperty(ref _lengthText, value);
        }

        bool _uppercaseOn;
        public bool UppercaseOn

        {
            get => Preferences.Get(nameof(UppercaseOn), true);
            set
            {
                SetProperty(ref _uppercaseOn, value);
                Preferences.Set(nameof(UppercaseOn), value);
                GenerateNew();
            }
        }

        bool _lowercaseOn;
        public bool LowercaseOn

        {
            get => Preferences.Get(nameof(LowercaseOn), true);
            set
            {
                SetProperty(ref _lowercaseOn, value);
                Preferences.Set(nameof(LowercaseOn), value);
                GenerateNew();
            }
        }

        bool _numbersOn;
        public bool NumbersOn

        {
            get => Preferences.Get(nameof(NumbersOn), true);
            set
            {
                SetProperty(ref _numbersOn, value);
                Preferences.Set(nameof(NumbersOn), value);
                GenerateNew();
            }
        }

        bool _specialCharsOn;
        public bool SpecialCharsOn
        {
            get => Preferences.Get(nameof(SpecialCharsOn), true);
            set
            {
                SetProperty(ref _specialCharsOn, value);
                Preferences.Set(nameof(SpecialCharsOn), value);
                GenerateNew();
            }
        }

        private async Task Copy()
        {
            await Clipboard.SetTextAsync(Password);
        }

        public void GenerateNew()
        {
            if (!NumbersOn && !SpecialCharsOn && !UppercaseOn && !LowercaseOn)
                LowercaseOn = true;

            Password = PasswordGeneratorService.GeneratePassword(Length, NumbersOn, SpecialCharsOn, UppercaseOn, LowercaseOn);
        }
    }
}
