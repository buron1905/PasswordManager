#if __ANDROID__
using Android.Content.Res;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using PasswordManager.MAUI.Handlers;
#endif

namespace PasswordManager.MAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            ModifyCustomControls();

            MainPage = new AppShell();
        }

        private void ModifyCustomControls()
        {
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderLine", (handler, view) =>
            {
                if (view is BorderlessEntry)
                {
#if ANDROID
                    (handler.PlatformView as Android.Views.View).SetBackgroundColor(Microsoft.Maui.Graphics.Colors.Transparent.ToAndroid());
#endif
                }
            });

            Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping("NoUnderLine", (handler, view) =>
            {
                if (view is BorderlessEditor)
                {
#if ANDROID
                    (handler.PlatformView as Android.Views.View).SetBackgroundColor(Microsoft.Maui.Graphics.Colors.Transparent.ToAndroid());
#endif
                }
            });

            //            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
            //            {
            //                if (view is BorderlessEntry)
            //                {
            //#if __ANDROID__
            //                //handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
            //            (handler.PlatformView as Android.Views.View).SetBackgroundColor(Microsoft.Maui.Graphics.Colors.Transparent.ToAndroid());
            //#elif __IOS__
            //                    handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
            //#elif WINDOWS
            //                handler.PlatformView.FontWeight = Microsoft.UI.Text.FontWeights.Thin;
            //#endif
            //                }
            //            });
        }
    }
}