using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Controls;

namespace PasswordManager.Views.Controls
{
    public partial class ReadOnlyDetailEntry : ContentView
    {
        public ReadOnlyDetailEntry()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(TogglePasswordEntry));

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(TogglePasswordEntry),
                defaultBindingMode: BindingMode.TwoWay);

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private async void OnCopyButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Text))
                return;

            await Clipboard.SetTextAsync(Text);
        }
    }
}