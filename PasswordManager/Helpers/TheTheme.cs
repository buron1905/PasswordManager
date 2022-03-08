using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Helpers
{
    public enum Theme
    {
        Unspecified,
        Light,
        Dark
    }

    public static class TheTheme
    {
        public static void SetTheme()
        {
            switch (Settings.Theme)
            {
                //default
                case Theme.Unspecified:
                    App.Current.UserAppTheme = OSAppTheme.Unspecified;
                    break;
                //light
                case Theme.Light:
                    App.Current.UserAppTheme = OSAppTheme.Light;
                    break;
                //dark
                case Theme.Dark:
                    App.Current.UserAppTheme = OSAppTheme.Dark;
                    break;
            }

            //Application.Current.Resources.MergedDictionaries
            //https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/theming/theming

            //var e = DependencyService.Get<IEnvironment>();
            //if (App.Current.RequestedTheme == OSAppTheme.Dark)
            //{
            //    e?.SetStatusBarColor(Color.Black, false);
            //    if (nav != null)
            //    {
            //        nav.BarBackgroundColor = Color.Black;
            //        nav.BarTextColor = Color.White;
            //    }
            //}
            //else
            //{
            //    e?.SetStatusBarColor(Color.White, true);
            //    if (nav != null)
            //    {
            //        nav.BarBackgroundColor = Color.White;
            //        nav.BarTextColor = Color.Black;
            //    }
            //}
        }
    }
}
