using PasswordManager.MAUI.Services;

namespace PasswordManager.MAUI.Views.Controls;

public partial class TogglePasswordWithClipboardEntry : ContentView
{
    public TogglePasswordWithClipboardEntry()
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
        BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(TogglePasswordEntry),
            defaultValue: Keyboard.Default);

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set
        {
            SetValue(TextProperty, value);
            RefreshPlaceholder();
        }
    }

    public bool HidePassword
    {
        get => (bool)GetValue(HidePasswordProperty);
        set => SetValue(HidePasswordProperty, value);
    }
    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }

    private void OnToggleButtonClicked(object sender, EventArgs e)
    {
        HidePassword = !HidePassword;
    }

    private async void OnClipboardButtonClicked(object sender, EventArgs e)
    {
        await Clipboard.SetTextAsync(Text);
        await PopupService.ShowToast("Copied to clipboard");
    }

    protected void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        txtEntry.Focus();
    }

    protected void txtEntry_Focused(object sender, FocusEventArgs e)
    {
        RefreshPlaceholder();
    }

    protected void txtEntry_Unfocused(object sender, FocusEventArgs e)
    {
        RefreshPlaceholder();
    }

    protected void RefreshPlaceholder()
    {
        if (!string.IsNullOrWhiteSpace(Text) || txtEntry.IsFocused)
        {
            lblPlaceholder.FontSize = 11;
            lblPlaceholder.TranslateTo(0, -20, 80, easing: Easing.Linear);
        }
        else
        {
            lblPlaceholder.FontSize = 15;
            lblPlaceholder.TranslateTo(0, 0, 80, easing: Easing.Linear);
        }
    }
}