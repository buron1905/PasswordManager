namespace PasswordManager.MAUI.Views.Controls
{
    public partial class PasswordDetailEntry : ContentView
    {
        public PasswordDetailEntry()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(TogglePasswordEntry));

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(TogglePasswordEntry),
                defaultBindingMode: BindingMode.TwoWay);

        public static readonly BindableProperty HidePasswordProperty =
            BindableProperty.Create(nameof(HidePassword), typeof(bool), typeof(TogglePasswordEntry),
                defaultValue: true);

        public static readonly BindableProperty HidePasswordColorProperty =
            BindableProperty.Create(nameof(HidePasswordColor), typeof(Color), typeof(TogglePasswordEntry),
                defaultValue: Color.FromRgb(0, 0, 0));

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

        public bool HidePassword
        {
            get => (bool)GetValue(HidePasswordProperty);
            set => SetValue(HidePasswordProperty, value);
        }

        public Color HidePasswordColor
        {
            get => (Color)GetValue(HidePasswordColorProperty);
            set => SetValue(HidePasswordColorProperty, value);
        }


        private void OnImageButtonClicked(object sender, EventArgs e)
        {
            HidePassword = !HidePassword;
        }

        private async void OnCopyButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Text))
                return;

            await Clipboard.SetTextAsync(Text);
        }
    }
}


