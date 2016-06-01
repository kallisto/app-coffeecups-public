using ImageCircle.Forms.Plugin.Abstractions;
using Android.Runtime;
using Android.Views;
using Android.Util;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using ImageCircle.Forms.Plugin.Droid;
using System;
using System.ComponentModel;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(CircleImage), typeof(ImageCircleRenderer))]
namespace ImageCircle.Forms.Plugin.Droid
{
	/// <summary>
	/// ImageCircle Implementation
	/// </summary>
	[Preserve(AllMembers = true)]
	public class ImageCircleRenderer : ImageRenderer
	{
		Path path = new Path();
		Paint paint = new Paint();


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

			if (e.OldElement == null)
			{
				//Only enable hardware accelleration on lollipop
				if ((int)Android.OS.Build.VERSION.SdkInt < 21)
				{
					SetLayerType(LayerType.Software, null);
				}

			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == CircleImage.BorderColorProperty.PropertyName ||
			  e.PropertyName == CircleImage.BorderThicknessProperty.PropertyName ||
			  e.PropertyName == CircleImage.FillColorProperty.PropertyName)
			{
				this.Invalidate();
			}
		}

		protected override bool DrawChild(Canvas canvas, Android.Views.View child, long drawingTime)
		{
			try
			{

				var radius = Math.Min(Width, Height) / 2;

				var borderThickness = (float)((CircleImage)Element).BorderThickness;

				int strokeWidth = 0;

				if (borderThickness > 0)
				{
					var logicalDensity = Xamarin.Forms.Forms.Context.Resources.DisplayMetrics.Density;
					strokeWidth = (int)Math.Ceiling(borderThickness * logicalDensity + .5f);
				}

				radius -= strokeWidth;

				path.Reset();
				path.AddCircle(Width / 2.0f, Height / 2.0f, radius, Path.Direction.Ccw);

				paint.AntiAlias = true;

				if (strokeWidth > 0.0f)
				{
					paint.SetStyle(Paint.Style.Stroke);
					paint.Color = ((CircleImage)Element).BorderColor.ToAndroid();
					paint.StrokeWidth = strokeWidth;
					canvas.DrawPath(path, paint);
				}

				paint.SetStyle(Paint.Style.Fill);
				paint.Color = ((CircleImage)Element).FillColor.ToAndroid();
				canvas.DrawPath(path, paint);

				canvas.Save();
				canvas.ClipPath(path);
				var result = base.DrawChild(canvas, child, drawingTime);
				canvas.Restore();

				paint.Color = Android.Graphics.Color.PapayaWhip;
				paint.SetStyle(Paint.Style.Fill);
				paint.Alpha = 95;
				canvas.DrawPath(path, paint);

				return result;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Unable to create circle image: " + ex);
			}

			return base.DrawChild(canvas, child, drawingTime);
		}
	}
}