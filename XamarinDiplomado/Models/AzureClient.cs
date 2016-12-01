using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace XamarinDiplomado.Models
{
	public class AzureClient
	{
		private IMobileServiceClient _remoteClient;
		private	IMobileServiceSyncTable<ToDoItem> _todoItemsTable;
		private const string DB_PATH = "localstore.db";

		public AzureClient()
		{
			_remoteClient = new MobileServiceClient("http://midemo.azurewebsites.net/");
			var store = new MobileServiceSQLiteStore(DB_PATH);
			store.DefineTable<ToDoItem>();
			_remoteClient.SyncContext.InitializeAsync(store);
			_todoItemsTable = _remoteClient.GetSyncTable<ToDoItem>();
		}

		public async Task<IEnumerable<ToDoItem>> GetItems()
		{
			var items = new ToDoItem[0];

			try
			{
				if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
				{
					await this.SyncAsync();
				}

				return await _todoItemsTable.ToEnumerableAsync();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Error", ex.Message);
			}


			return items;
		}

		public async Task SyncAsync()
		{
			ReadOnlyCollection<MobileServiceTableOperationError> errors = null;

			try
			{
				await _remoteClient.SyncContext.PushAsync();
				await _todoItemsTable.PullAsync("allToDoItems",
												_todoItemsTable.CreateQuery());
			}
			catch (MobileServicePushFailedException ex)
			{
				if (ex != null)	errors = ex.PushResult.Errors;
			}
		}

		public async void PurgeData()
		{

			await _todoItemsTable.PurgeAsync("allToDoItems", _todoItemsTable.CreateQuery(), new System.Threading.CancellationToken());


		}

		public async void AddToDoItem(ToDoItem item)
		{
			await _todoItemsTable.InsertAsync(item);
		}
	}
}
