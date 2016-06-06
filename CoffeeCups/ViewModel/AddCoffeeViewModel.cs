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
	public class AddCoffeeViewModel: BaseViewModel
	{
		ICommand takePhotoCommand;
		ICommand addCoffeeCommand;
		string loadingMessage;
		CupOfCoffee coffee;

		public AddCoffeeViewModel()
		{
			coffee = new CupOfCoffee {
				Name = "",
				Notes = "",
				Image = "kopi_luwak.jpg"
					// TODO: the image string will be placeholder.jpg
			};
		}

		App app = Application.Current as App;

		public CupOfCoffee Coffee
		{
			private set { SetProperty(ref coffee, value); }
			get { return coffee; }
		}

		public ICommand TakePhotoCommand =>
			takePhotoCommand ?? (takePhotoCommand = new Command(async () => await ExecuteTakePhotoCommandAsync()));

		public ICommand AddCoffeeCommand =>
			addCoffeeCommand ?? (addCoffeeCommand = new Command(async () => await ExecuteAddCoffeeCommandAsync()));

		async Task ExecuteTakePhotoCommandAsync()
		{
			if (IsBusy)
				return;
			try
			{
				// take a photo and save it
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

				IsBusy = true;
				var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
				{
					Directory = "Sample",
					Name = "test.jpg"
				});

				if (file == null)
					return;

				coffee.Image = file.Path;

				//TODO make this so it saves the image
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
				//TODO: see if we need to log user in here
				LoadingMessage = "Adding Coffee...";
				IsBusy = true;

				//TODO find how to pass image
				await app.azureService.AddCoffee(coffee);
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

		public string LoadingMessage
		{
			get { return loadingMessage; }
			set { SetProperty(ref loadingMessage, value); }
		}
	}
}

