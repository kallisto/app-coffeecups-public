using System;
using Humanizer;

namespace CoffeeCups
{
	public class CupOfCoffee
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
		public string Image { get; set; }

		[Newtonsoft.Json.JsonIgnore]
		public string TimeDisplay => DateUtc.ToLocalTime().ToString("t");

		[Newtonsoft.Json.JsonIgnore]
		public string DateDisplay => $"{DateUtc.DayOfWeek.ToString()}, {DateUtc.ToLocalTime().ToString("MMMM")} {DateUtc.ToLocalTime().Day.Ordinalize()}";
	}
}

