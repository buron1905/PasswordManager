using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Helpers
{
    public static class TheTheme
    {
        public static void SetTheme()
        {
            switch (Settings.Theme)
            {
                //default
                case 0:
                    App.Current.UserAppTheme = OSAppTheme.Unspecified;
                    break;
                //light
                case 1:
                    App.Current.UserAppTheme = OSAppTheme.Light;
                    break;
                //dark
                case 2:
                    App.Current.UserAppTheme = OSAppTheme.Dark;
                    break;
            }

            //Application.Current.Resources.MergedDictionaries
            //https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/theming/theming

            //var nav = App.Current.MainPage as NavigationPage;
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
