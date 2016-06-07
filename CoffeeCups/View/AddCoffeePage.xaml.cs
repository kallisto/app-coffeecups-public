using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CoffeeCups
{
	public partial class AddCoffeePage : ContentPage
	{

		public AddCoffeePage(AddCoffeeViewModel vm)
		{
			InitializeComponent();

			BindingContext = vm;

			//entry.Effects.Add(Effect.Resolve("Xamarin.BackgroundColorEffect"));
			//description.Effects.Add(Effect.Resolve("Xamarin.BackgroundColorEffect"));

		}
	}
}

