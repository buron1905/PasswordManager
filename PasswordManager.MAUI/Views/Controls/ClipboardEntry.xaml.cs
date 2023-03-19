using PasswordManager.MAUI.Services;

namespace PasswordManager.MAUI.Views.Controls;

public partial class ClipboardEntry : ContentView
{
    public ClipboardEntry()
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

    private async void OnClipboardButtonClicked(object sender, EventArgs e)
    {
        await Clipboard.SetTextAsync(Text);
        await PopupService.ShowToast("Copied to clipboard");
    }
}