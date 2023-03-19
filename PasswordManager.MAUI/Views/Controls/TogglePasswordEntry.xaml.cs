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

        public static readonly BindableProperty KeyboardProperty =
            BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(OutLinedEntry),
                defaultValue: Keyboard.Default);

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

        public Keyboard Keyboard
        {
            get => (Keyboard)GetValue(KeyboardProperty);
            set => SetValue(KeyboardProperty, value);
        }

        protected void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            txtEntry.Focus();
        }

        protected void txtEntry_Focused(object sender, FocusEventArgs e)
        {
            lblPlaceholder.FontSize = 11;
            lblPlaceholder.TranslateTo(0, -20, 80, easing: Easing.Linear);
        }

        protected void txtEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Text))
            {
                lblPlaceholder.FontSize = 11;
                lblPlaceholder.TranslateTo(0, -20, 80, easing: Easing.Linear);
            }
            else
            {
                lblPlaceholder.FontSize = 11;
                lblPlaceholder.TranslateTo(0, 0, 80, easing: Easing.Linear);
            }
        }

    }
}




