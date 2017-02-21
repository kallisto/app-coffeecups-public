using System;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.Diagnostics;
using Xamarin.Forms;
using CoffeeCups.Helpers;

namespace CoffeeCups
{
	public class AzureService
	{
		bool isInitialized;
		public MobileServiceClient MobileService { get; set; }
		IMobileServiceSyncTable<CupOfCoffee> coffeeTable;

		public async Task Initialize()
		{
			if (isInitialized)
				return;

			//Create our client
			MobileService = new MobileServiceClient("https://YOU-RURL-HERE.azurewebsites.net", null);

			const string path = "syncstore_new.db";

			//setup our local sqlite store and intialize our table
			var store = new MobileServiceSQLiteStore(path);
			store.DefineTable<CupOfCoffee>();
			await MobileService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

			//Get our sync table that will call out to azure
			coffeeTable = MobileService.GetSyncTable<CupOfCoffee>();

			isInitialized = true;
		}

		public async Task<IEnumerable<CupOfCoffee>> GetCoffees()
		{
			await Initialize();
			await SyncCoffee();
			return await coffeeTable.OrderByDescending(c => c.DateUtc).ToEnumerableAsync();
		}

		public async Task AddCoffee(CupOfCoffee coffee)
		{
			await Initialize();

			coffee.OS = Device.OS.ToString();
			coffee.DateUtc = DateTime.UtcNow;

			await coffeeTable.InsertAsync(coffee);

			//Synchronize coffee
			await SyncCoffee();
		}

		public async Task SyncCoffee()
		{
			try
			{
				//pull down all latest changes and then push current coffees up
				await coffeeTable.PullAsync("allCoffees", coffeeTable.CreateQuery());
				await MobileService.SyncContext.PushAsync();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Unable to sync coffees, that is alright as we have offline capabilities: " + ex);
			}
		}
	}
}

