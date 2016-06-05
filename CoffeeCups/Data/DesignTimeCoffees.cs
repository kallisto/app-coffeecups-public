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
					new CupOfCoffee { Name = "Hacienda", Notes = "Tangy, citrus", DateUtc = Convert.ToDateTime("04/16/2016 15:50:50"), Image = "coffee0.jpg"},
					new CupOfCoffee { Name = "Costa Rica", Notes = "Almonds, toffee", DateUtc = Convert.ToDateTime("04/16/2016 16:10:20"), Image = "coffee1.jpg"},
					new CupOfCoffee { Name = "Kona", Notes = "Caramel overtones", DateUtc = Convert.ToDateTime("04/20/2016 08:17:20"), Image = "coffee2.jpg"},
					new CupOfCoffee { Name = "Los Planes", Notes = "Delightful, malty", DateUtc = Convert.ToDateTime("04/20/2016 20:10:25"), Image = "coffee3.jpg"},
					new CupOfCoffee { Name = "Blue Mountain", Notes = "Exquisitely enzymatic", DateUtc = Convert.ToDateTime("04/20/2016 10:11:20"), Image = "coffee4.jpg"},
					new CupOfCoffee { Name = "Kopi Luwak", Notes = "Balanced, earthy", DateUtc = Convert.ToDateTime("04/20/2016 08:17:20"), Image = "coffee5.jpg"},
					new CupOfCoffee { Name = "Peruvian", Notes = "Hint of torched forest pine", DateUtc = Convert.ToDateTime("04/24/2016 20:10:25"), Image = "coffee6.jpg"},
					new CupOfCoffee { Name = "Sumatra", Notes = "Like an orchestra of chocolate and dreams", DateUtc = Convert.ToDateTime("04/24/2016 8:12:20"), Image = "coffee7.jpg"},
					new CupOfCoffee { Name = "Ethiopian", Notes = "Subtle suggestions of a starry night by the campfire", DateUtc = Convert.ToDateTime("04/24/2016 5:16:20"), Image = "coffee8.jpg"},
				}.GroupBy(c => c.DateDisplay)
				 .Reverse()
				 .Select(g => new Grouping<string, CupOfCoffee>(g.Key, g))
				 .ToArray();	
			}
		}
	}
}
