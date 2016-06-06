using System;
using CoffeeCups.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("Xamarin")]
[assembly: ExportEffect(typeof(BackgroundColorEffect), "BackgroundColorEffect")]

namespace CoffeeCups.iOS
{
	public class BackgroundColorEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			try
			{
				Control.BackgroundColor = UIColor.FromRGB(204, 153, 255);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
			}
		}

		protected override void OnDetached()
		{

		}
	}
}