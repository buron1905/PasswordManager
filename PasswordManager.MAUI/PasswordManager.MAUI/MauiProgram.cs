namespace PasswordManager.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("fa-6-Brands-Regular-400.otf", "FAR");
                    fonts.AddFont("fa-6-Free-Regular-400.otf", "FAB");
                    fonts.AddFont("fa-6-Free-Solid-900.otf", "FAS");
                });
            
            //DatabaseService.Init();

            return builder.Build();
        }
    }
}