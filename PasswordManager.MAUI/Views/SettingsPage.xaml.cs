using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PasswordManager.MAUI.ViewModels;
using PasswordManager.MAUI.Helpers;

namespace PasswordManager.MAUI.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsViewModel ViewModel { get; set; }
        public SettingsPage()
        {
            InitializeComponent();
            ViewModel = BindingContext as SettingsViewModel;

            //switch (Settings.Theme)
            //{
            //    case 0:
            //        RadioButtonSystem.IsChecked = true;
            //        break;
            //    case 1:
            //        RadioButtonLight.IsChecked = true;
            //        break;
            //    case 2:
            //        RadioButtonDark.IsChecked = true;
            //        break;
            //}
        }

        private bool loaded = true;

        //void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        //{
        //    if (!loaded)
        //        return;

        //    if (!e.Value)
        //        return;

        //    var val = (sender as RadioButton)?.Value as string;
        //    if (string.IsNullOrWhiteSpace(val))
        //        return;

        //    switch (val)
        //    {
        //        case "System":
        //            Settings.Theme = Theme.Unspecified;
        //            break;
        //        case "Light":
        //            Settings.Theme = Theme.Light;
        //            break;
        //        case "Dark":
        //            Settings.Theme = Theme.Dark;
        //            break;
        //    }

        //    TheTheme.SetTheme();
        //}
    }
}
