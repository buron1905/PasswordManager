using PasswordManager.MAUI.Services;

namespace PasswordManager.MAUI.Views.Controls;

public partial class UrlEntry : ContentView
{
    public UrlEntry()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(TogglePasswordEntry));

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(TogglePasswordEntry),
            defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty KeyboardProperty =
        BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(TogglePasswordEntry),
            defaultValue: Keyboard.Url);

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

    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }

    private async void OnGoToButtonClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(Text))
        {
            var uriString = Text.Trim();
            if (!uriString.StartsWith("http://") && !uriString.StartsWith("https://"))
                uriString = "http://" + uriString;

            try
            {
                await Browser.Default.OpenAsync(new Uri(uriString), BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                // unexpected error (e.g. no browser is installed)
                await AlertService.ShowToast("Error with opening URL");
            }
        }
    }

    private async void OnClipboardButtonClicked(object sender, EventArgs e)
    {
        await Clipboard.SetTextAsync(Text);
        await AlertService.ShowToast("Copied to clipboard");
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