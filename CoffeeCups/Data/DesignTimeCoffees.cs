using System;
using System.Linq;
using MvvmHelpers;

namespace CoffeeCups
{
	public class DesignTimeCoffees
	{
		public static Grouping<string, CupOfCoffee>[] Coffees
		{
			get
			{
				return new CupOfCoffee[] {
					new CupOfCoffee { MadeAtHome = true, DateUtc = DateTime.Now },
					new CupOfCoffee { MadeAtHome = false, DateUtc = DateTime.Now.Subtract (TimeSpan.FromDays(1))},
					new CupOfCoffee { MadeAtHome = false, DateUtc = DateTime.Now.Subtract (TimeSpan.FromDays(2)) },
					new CupOfCoffee { MadeAtHome = true, DateUtc = DateTime.Now.Subtract (TimeSpan.FromDays(3)) },
					new CupOfCoffee { MadeAtHome = false, DateUtc = DateTime.Now.Subtract (TimeSpan.FromDays(3)) },
					new CupOfCoffee { MadeAtHome = true, DateUtc = DateTime.Now.Subtract (TimeSpan.FromDays(2)) },
					new CupOfCoffee { MadeAtHome = true, DateUtc = DateTime.Now.Subtract (TimeSpan.FromDays(4)) },
					new CupOfCoffee { MadeAtHome = false, DateUtc = DateTime.Now.Subtract (TimeSpan.FromDays(3)) },
					new CupOfCoffee { MadeAtHome = true, DateUtc = DateTime.Now.Subtract (TimeSpan.FromDays(1)) }
				}.GroupBy(t => t.DateUtc.ToString())
				 .Select(t => new Grouping<string, CupOfCoffee>(t.Key, t))
				 .ToArray();
			}
		}
	}
}
