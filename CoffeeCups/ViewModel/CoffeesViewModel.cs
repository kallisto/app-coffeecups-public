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
using Plugin.Media;

namespace CoffeeCups
{
	public class CoffeesViewModel : BaseViewModel
	{
		AzureService azureService;
		string loadingMessage;
		ICommand loadCoffeesCommand;
		ICommand takePhotoCommand;
		ICommand addCoffeeCommand;

		public CoffeesViewModel()
		{
			azureService = new AzureService();
		}

		public ObservableRangeCollection<CupOfCoffee> Coffees { get; } = new ObservableRangeCollection<CupOfCoffee>();
		public ObservableRangeCollection<Grouping<string, CupOfCoffee>> CoffeesGrouped { get; } = new ObservableRangeCollection<Grouping<string, CupOfCoffee>>();

		public ICommand LoadCoffeesCommand =>
			loadCoffeesCommand ?? (loadCoffeesCommand = new Command(async () => await ExecuteLoadCoffeesCommandAsync()));

		public ICommand TakePhotoCommand =>
			takePhotoCommand ?? (takePhotoCommand = new Command(async () => await ExecuteTakePhotoCommandAsync()));

		public ICommand AddCoffeeCommand =>
			addCoffeeCommand ?? (addCoffeeCommand = new Command(async () => await ExecuteAddCoffeeCommandAsync()));

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

		async Task ExecuteLoadCoffeesCommandAsync()
		{
			if (IsBusy)
				return;
			try
			{
				if (!Settings.IsLoggedIn)
				{
					// log in user
					await azureService.Initialize();
					var user = await DependencyService.Get<IAuthentication>().LoginAsync(
						azureService.MobileService,
						MobileServiceAuthenticationProvider.MicrosoftAccount);
					if (user == null)
						return;
				}

				LoadingMessage = "Loading Coffees...";
				IsBusy = true;
				var coffees = await azureService.GetCoffees();
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

		async Task ExecuteTakePhotoCommandAsync()
		{
			if (IsBusy)
				return;
			try
			{
				// take a photo and 
				await CrossMedia.Current.Initialize();

				if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
				{
					MessagingService.Current.SendMessage<MessagingServiceAlert>("message", new MessagingServiceAlert
					{
						Cancel = "OK",
						Message = "No camera available",
						Title = "Camera"
					});
					return;
				}

				var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
				{
					Directory = "Sample",
					Name = "test.jpg"
				});

				if (file == null)
					return;

				MessagingService.Current.SendMessage<MessagingServiceAlert>("message", new MessagingServiceAlert
				{
					Cancel = "OK",
					Message = $"{file.Path}",
					Title = "Location"
				});

			}
			catch (Exception ex)
			{
				Debug.WriteLine("OH NO!" + ex);
			}
			finally
			{
				IsBusy = false;
			}
		}

		async Task ExecuteAddCoffeeCommandAsync()
		{
			if (IsBusy)
				return;
			try
			{
				if (!Settings.IsLoggedIn)
				{
					// log user in
					await azureService.Initialize();
					var user = await DependencyService.Get<IAuthentication>().LoginAsync(azureService.MobileService, MobileServiceAuthenticationProvider.MicrosoftAccount);
					if (user == null)
						return;

					LoadingMessage = "Adding Coffee...";
					IsBusy = true;

					// load coffees from Azure
					var coffees = await azureService.GetCoffees();
					Coffees.ReplaceRange(coffees);

					SortCoffees();
				}
				else
				{
					LoadingMessage = "Adding Coffee...";
					IsBusy = true;
				}

				var coffee = await azureService.AddCoffee();
				Coffees.Add(coffee);
				SortCoffees();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("OH NO!" + ex);
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}

