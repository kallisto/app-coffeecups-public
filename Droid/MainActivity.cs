using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using ImageCircle.Forms.Plugin.Droid;
using Xamarin.Forms.Platform.Android;

namespace CoffeeCups.Droid
{
	[Activity (Label = "CoffeeCups", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{

            FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
            FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;
         
            base.OnCreate (bundle);
			global::Xamarin.Forms.Forms.Init (this, bundle);

            FormsToolkit.Droid.Toolkit.Init();
            ImageCircleRenderer.Initialize();
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

			LoadApplication (new App ());
		}
	}
}
