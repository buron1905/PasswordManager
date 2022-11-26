using PasswordManager.MAUI.Helpers;
using MvvmHelpers;

namespace PasswordManager.MAUI.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            Title = "Settings";

        }

        private Theme _selectedTheme;
        public Theme SelectedTheme
        {
            get => _selectedTheme;
            set => SetProperty(ref _selectedTheme, value);
        }

        public List<string> Themes
        {
            get => Enum.GetNames(typeof(Theme)).ToList();
        }

        public void DoSomethingWithBreed()
        {

        }

    }
}
