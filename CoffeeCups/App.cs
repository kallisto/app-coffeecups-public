using System;

using Xamarin.Forms;
using FormsToolkit;

namespace CoffeeCups
{
	public class App : Application
	{
		//TODO fix when view models get created
		public AzureService azureService;
		CoffeesViewModel coffeesViewModel;

		public App()
		{
			// The root page of your application
			azureService = new AzureService();
			coffeesViewModel = new CoffeesViewModel();
			MainPage = new NavigationPageNoLine(new CoffeesPage(coffeesViewModel))
			{
				BarTextColor = Color.White,
				BarBackgroundColor = Color.Maroon
			};
		}

		protected override void OnStart()
		{
			MessagingService.Current.Subscribe<MessagingServiceAlert>("message", async (m, info) =>
				{
					var task = Application.Current?.MainPage?.DisplayAlert(info.Title, info.Message, info.Cancel);

					if (task == null)
						return;

					await task;
					info?.OnCompleted?.Invoke();
				});

		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

