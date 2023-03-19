namespace PasswordManager.MAUI.Views.Controls
{
    public partial class TogglePasswordEntry : ContentView
    {
        public TogglePasswordEntry()
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

        private void OnToggleButtonClicked(object sender, EventArgs e)
        {
            HidePassword = !HidePassword;
        }
    }
}




