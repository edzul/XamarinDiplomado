using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using XamarinDiplomado.Models;
using Xamarin.Forms;

namespace XamarinDiplomado.ViewModels
{
	public class ToDoItemVM : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private AzureClient _client;
		public ObservableCollection<ToDoItem> Items {get; set;}
		public Command GetItemsCommand { get; set;}
		private bool _isBusy;

		public ToDoItemVM()
		{
			Items = new ObservableCollection<ToDoItem>();
			_client = new AzureClient();
			GetItemsCommand = new Command(() => Load());

		}

		public bool IsBusy
		{
			get { return _isBusy;}
			set
			{
				IsBusy = value;
				OnPropertyChanged();
				GetItemsCommand.ChangeCanExecute();
			}
		}

		public void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public async void Load()
		{
			if (!IsBusy)
			{
				IsBusy = true;

				var result = await _client.GetItems();
				Items.Clear();

				foreach (var i in result)
				{
					Items.Add(i);
				}

				IsBusy = false;
			}
		}
	}
}
