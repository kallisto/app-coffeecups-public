using System;
using MvvmHelpers;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using FormsToolkit;
using System.Linq;
using CoffeeCups.Helpers;
using Microsoft.WindowsAzure.MobileServices;

namespace CoffeeCups
{
	public class CoffeesViewModel : BaseViewModel
	{
		ICommand loadCoffeesCommand;
		string loadingMessage;

		public CoffeesViewModel()
		{
		}

		App app = Application.Current as App;
		public ObservableRangeCollection<CupOfCoffee> Coffees { get; } = new ObservableRangeCollection<CupOfCoffee>();
		public ObservableRangeCollection<Grouping<string, CupOfCoffee>> CoffeesGrouped { get; } = new ObservableRangeCollection<Grouping<string, CupOfCoffee>>();

		public ICommand LoadCoffeesCommand =>
			loadCoffeesCommand ?? (loadCoffeesCommand = new Command(async () => await ExecuteLoadCoffeesCommandAsync()));

		async Task ExecuteLoadCoffeesCommandAsync()
		{
			if (IsBusy)
				return;
			try
			{
				if (!Settings.IsLoggedIn)
				{
					// log in user
					await app.azureService.Initialize();
					var user = await DependencyService.Get<IAuthentication>().LoginAsync(
						app.azureService.MobileService,
						MobileServiceAuthenticationProvider.MicrosoftAccount);
					if (user == null)
						return;
				}

				LoadingMessage = "Loading Coffees...";
				IsBusy = true;
				var coffees = await app.azureService.GetCoffees();
				Coffees.ReplaceRange(coffees);

				SortCoffees();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("OH NO!" + ex);
				MessagingService.Current.SendMessage<MessagingServiceAlert>("message", new MessagingServiceAlert
				{
					Cancel = "OK",
					Message = "Unable to sync coffees, you may be offline",
					Title = "Coffee sync Error"
				});
			}
			finally
			{
				IsBusy = false;
			}
		}

		public string LoadingMessage
		{
			get { return loadingMessage; }
			set { SetProperty(ref loadingMessage, value); }
		}

		void SortCoffees()
		{
			var groups = from coffee in Coffees
						 orderby coffee.DateUtc descending
						 group coffee by coffee.DateDisplay
				into coffeeGroup
						 select new Grouping<string, CupOfCoffee>($"{coffeeGroup.Key} ({coffeeGroup.Count()})", coffeeGroup);
			CoffeesGrouped.ReplaceRange(groups);
		}
	}
}