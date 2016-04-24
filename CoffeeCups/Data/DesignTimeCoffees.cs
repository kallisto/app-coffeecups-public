using System;
using System.Linq;
using MvvmHelpers;

namespace CoffeeCups
{
	// Mock data to populate the design-time list of coffees
	public class DesignTimeCoffees
	{
		// A collection of fake data to play with at design-time
		public static Grouping<string, CupOfCoffee>[] Coffees
		{
			get
			{
				return new CupOfCoffee[] {
					new CupOfCoffee { Type = "Hacienda", Notes = "Tangy, citrus", DateUtc = Convert.ToDateTime("04/16/2016 15:50:50"), Image = "home.png"},
					new CupOfCoffee { Type = "Costa Rica", Notes = "Almonds, toffee", DateUtc = Convert.ToDateTime("04/17/2016 16:10:20"), Image = "out.png"},
					new CupOfCoffee { Type = "Kona", Notes = "Caramel notes", DateUtc = Convert.ToDateTime("04/20/2016 08:17:20"), Image = "coffee1.jpg"},
					new CupOfCoffee { Type = "Los Planes", Notes = "Delightful", DateUtc = Convert.ToDateTime("04/21/2016 20:10:25"), Image = "coffee2.jpg"},
					new CupOfCoffee { Type = "Blue Mountain", Notes = "Chocolatey", DateUtc = Convert.ToDateTime("04/22/2016 10:11:20"), Image = "coffee3.jpg"},
				}.GroupBy(t => t.DateDisplay)
				 .Select(t => new Grouping<string, CupOfCoffee>(t.Key, t))
				 .ToArray();
			}
		}
	}
}
