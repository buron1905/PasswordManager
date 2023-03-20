using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.MAUI.Helpers;

namespace PasswordManager.MAUI.ViewModels
{
    public partial class SettingsViewModel : BaseViewModel
    {
        #region Properties

        [ObservableProperty]
        Theme _selectedTheme;

        bool _askForPassword;
        public bool AskForPassword
        {
            get => Preferences.Get(nameof(AskForPassword), false);
            set
            {
                SetProperty(ref _askForPassword, value);
                Preferences.Set(nameof(AskForPassword), value);
            }
        }

        public List<string> Themes
        {
            get => Enum.GetNames(typeof(Theme)).ToList();
        }

        #endregion

        public SettingsViewModel()
        {
            Title = "Settings";
            SelectedTheme = Settings.Theme;
        }

        #region Commands

        [RelayCommand]
        public void ThemeChanged()
        {
            Settings.Theme = SelectedTheme;
            TheTheme.SetTheme();
        }

        #endregion
    }
}
