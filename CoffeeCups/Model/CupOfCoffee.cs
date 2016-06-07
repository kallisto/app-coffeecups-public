using System;
using Humanizer;
using Xamarin.Forms;

namespace CoffeeCups
{
	public class CupOfCoffee : BindableObject
	{
		[Newtonsoft.Json.JsonProperty("Id")]
		public string Id { get; set; }

		[Microsoft.WindowsAzure.MobileServices.Version]
		public string AzureVersion { get; set; }

		/// <summary>
		/// Gets the OS of the user.
		/// </summary>
		/// <value>The OS.</value>
		public string OS { get; set; }

		/// <summary>
		/// Gets or sets the date UTC.
		/// </summary>
		/// <value>The date UTC.</value>
		public DateTime DateUtc { get; set; }

		/// <summary>
		/// Describes the type/origin of coffee.
		/// </summary>
		/// <value>Name of coffee</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets the description
		/// </summary>
		/// <value>notes</value>
		public string Notes { get; set; }

		/// <summary>
		/// Gets or sets the image path
		/// </summary>
		/// <value>Image file path</value>
		//public string Image { get; set; }
		public static readonly BindableProperty ImageProperty =
			BindableProperty.Create(
				nameof(Image), 
				typeof(string), 
				typeof(CupOfCoffee),
				default(string));

		public string Image
		{
			get { return (string)GetValue(ImageProperty); }
			set { SetValue(ImageProperty, value); }
		}


		[Newtonsoft.Json.JsonIgnore]
		public string TimeDisplay => DateUtc.ToLocalTime().ToString("t");

		[Newtonsoft.Json.JsonIgnore]
		public string DateDisplay => $"{DateUtc.DayOfWeek.ToString()}, {DateUtc.ToLocalTime().ToString("MMMM")} {DateUtc.ToLocalTime().Day.Ordinalize()}";
	}
}
