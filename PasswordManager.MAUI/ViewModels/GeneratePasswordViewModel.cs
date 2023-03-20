using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.MAUI.Services;
using Services;

namespace PasswordManager.MAUI.ViewModels
{
    public partial class GeneratePasswordViewModel : BaseViewModel
    {
        #region Properties

        [ObservableProperty]
        string _newPassword = string.Empty;

        [ObservableProperty]
        string _lengthText = string.Empty;

        int _length;
        public int Length
        {
            get => Preferences.Get(nameof(Length), 8);
            set
            {
                SetProperty(ref _length, value);
                Preferences.Set(nameof(Length), value);
                LengthText = $"Password length {_length}";
                Generate();
            }
        }

        bool _uppercaseOn;
        public bool UppercaseOn
        {
            get => Preferences.Get(nameof(UppercaseOn), true);
            set
            {
                SetProperty(ref _uppercaseOn, value);
                Preferences.Set(nameof(UppercaseOn), value);
                Generate();
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
                Generate();
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
                Generate();
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
                Generate();
            }
        }

        #endregion

        public GeneratePasswordViewModel()
        {
            Title = "Generator";
            Generate();
            LengthText = $"Password length {Length}";
        }

        #region Commands

        [RelayCommand]
        async Task Copy()
        {
            await Clipboard.SetTextAsync(NewPassword);
            await PopupService.ShowToast("Copied to clipboard");
        }

        [RelayCommand]
        void Generate()
        {
            if (!NumbersOn && !SpecialCharsOn && !UppercaseOn && !LowercaseOn)
                LowercaseOn = true;

            NewPassword = PasswordGeneratorService.GeneratePassword(Length, NumbersOn, SpecialCharsOn, UppercaseOn, LowercaseOn);
        }

        #endregion
    }
}
