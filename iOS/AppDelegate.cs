using Foundation;
using UIKit;
using ImageCircle.Forms.Plugin.iOS;

namespace CoffeeCups.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
			global::Xamarin.Forms.Forms.Init();
            ImageCircleRenderer.Initialize();

            LoadApplication(new App());

            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            SQLitePCL.CurrentPlatform.Init();
            FormsToolkit.iOS.Toolkit.Init();
            return base.FinishedLaunching(app, options);
        }
    }
}

