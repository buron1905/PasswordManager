using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Helpers
{
    public static class Settings
    {
        private const Theme _themeDefault = Theme.Unspecified;
        //const int theme = 0;
        public static Theme Theme
        {
            get => (Theme)Preferences.Get(nameof(Theme), (int)_themeDefault);
            set => Preferences.Set(nameof(Theme), (int)value);
        }
    }
}
