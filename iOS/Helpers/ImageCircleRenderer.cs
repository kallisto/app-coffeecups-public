using ImageCircle.Forms.Plugin.Abstractions;
using System;
using System.Linq;
using Xamarin.Forms;
using ImageCircle.Forms.Plugin.iOS;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;
using System.Diagnostics;
#if __UNIFIED__
using Foundation;
#else
using MonoTouch.Foundation;
#endif

[assembly: ExportRenderer(typeof(ImageCircle.Forms.Plugin.Abstractions.CircleImage), typeof(ImageCircleRenderer))]
namespace ImageCircle.Forms.Plugin.iOS
{
	/// <summary>
	/// ImageCircle Implementation
	/// </summary>
	[Preserve(AllMembers = true)]
	public class ImageCircleRenderer : ImageRenderer
	{
		/// <summary>
		/// Used for registration with dependency service
		/// </summary>
		public static void Initialize()
		{
			var temp = DateTime.Now;
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);
			if (Element == null)
				return;
			CreateCircle();
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == VisualElement.HeightProperty.PropertyName ||
				e.PropertyName == VisualElement.WidthProperty.PropertyName ||
				e.PropertyName == CircleImage.BorderColorProperty.PropertyName ||
				e.PropertyName == CircleImage.BorderThicknessProperty.PropertyName ||
				e.PropertyName == CircleImage.FillColorProperty.PropertyName)
			{
				CreateCircle();
			}
		}

		private void CreateCircle()
		{
			try
			{
				double min = Math.Min(Element.Width, Element.Height);
				Control.Layer.CornerRadius = (float)(min / 2.0);
				Control.Layer.MasksToBounds = false;
				Control.Layer.BorderColor = ((CircleImage)Element).BorderColor.ToCGColor();
				Control.Layer.BorderWidth = ((CircleImage)Element).BorderThickness;
				Control.BackgroundColor = ((CircleImage)Element).FillColor.ToUIColor();
				Control.ClipsToBounds = true;
				var tintLayer = Control.Layer.Sublayers?.FirstOrDefault();
				if (tintLayer == null)
				{
					tintLayer = new CoreAnimation.CALayer
					{
						BackgroundColor = UIKit.UIColor.Purple.CGColor,
						Opacity = 0.5f
					};
					Control.Layer.AddSublayer(tintLayer);
				}
				tintLayer.Frame = new CoreGraphics.CGRect(0, 0, Element.Width, Element.Height);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Unable to create circle image: " + ex);
			}
		}
	}
}