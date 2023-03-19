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
                await PopupService.ShowToast("Error with opening URL");
            }
        }
    }

    private async void OnClipboardButtonClicked(object sender, EventArgs e)
    {
        await Clipboard.SetTextAsync(Text);
        await PopupService.ShowToast("Copied to clipboard");
    }
}