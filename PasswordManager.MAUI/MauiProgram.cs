﻿using CommunityToolkit.Maui;
using PasswordManager.MAUI.Extensions;
using PasswordManager.MAUI.Handlers;

namespace PasswordManager.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("fa-6-Brands-Regular-400.otf", "FAR");
                    fonts.AddFont("fa-6-Free-Regular-400.otf", "FAB");
                    fonts.AddFont("fa-6-Free-Solid-900.otf", "FAS");
                });

            //builder.Configuration.GetAppSettings(builder.Services);

            builder
                .RegisterAppServices()
                .RegisterViewModels()
                .RegisterViews();

            builder.Services.AddSingleton<IHttpsClientHandlerService, HttpsClientHandlerService>();

            return builder.Build();
        }

    }
}