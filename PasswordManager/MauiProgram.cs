using MAUIDatabaseLib;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Essentials;
using Microsoft.Maui.Hosting;
using PasswordManager.Services;
using System;
using System.IO;

namespace PasswordManager
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
				});

            DatabaseService.Init();
            //DatabaseService.Init(Path.Combine(FileSystem.AppDataDirectory, "PasswordManagerLocal.db"));
            //DatabaseSQLiteMobile.Init(Path.GetFullPath(Path.Combine(FileSystem.AppDataDirectory, "PasswordManagerLocal.db")));
            //DatabaseSQLiteMobile.Init(Path.GetFullPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PasswordManagerLocal.db")));

            return builder.Build();
		}
	}
}