namespace PasswordManager.MAUI.Helpers
{
    public static class Settings
    {
        private const Theme _themeDefault = Theme.Unspecified;

        public static Theme Theme
        {
            get => (Theme)Preferences.Get(nameof(Theme), (int)_themeDefault);
            set => Preferences.Set(nameof(Theme), (int)value);
        }
    }
}
