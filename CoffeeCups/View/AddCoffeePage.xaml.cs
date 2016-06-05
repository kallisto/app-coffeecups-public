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
		}
	}
}

