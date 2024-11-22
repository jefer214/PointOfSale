namespace PointOfSaleApp.ViewModels
{
    using PointOfSaleApp.Models;
    using PointOfSaleApp.Services;
    using PointOfSaleApp.Views;
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class ClientsViewModel : BaseViewModel
    {
        readonly ApiService apiService = new ApiService();
        private ItemClient _selectedItem;

        public ObservableCollection<ItemClient> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<ItemClient> ItemTapped { get; }
        public Command<int> AddCommand { get; }

        public ClientsViewModel()
        {
            Title = "Clientes";
            Items = new ObservableCollection<ItemClient>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<ItemClient>(OnItemSelected);
            AddCommand = new Command<int>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();

                System.Collections.Generic.List<User> users = await App.Database.GetUsersAsync();

                if (!users.Any())
                {
                    users = await apiService.GetInfoUsersAsync();

                    await App.Database.SaveUserAsync(users);
                }

                foreach (var i in from item in users
                                  let i = new ItemClient()
                                  {
                                      Id = item.Id,
                                      Email = item.Email,
                                      FullName = item.FullName,
                                      Phone = item.Phone
                                  }
                                  select i)
                {
                    Items.Add(i);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public ItemClient SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await NavigationService2.NavigateToAsync<NewItemViewModel>();
        }

        async void OnItemSelected(ItemClient item)
        {
            if (item == null)
            {  
                return;
            }

            NavigationPage navigationPage = Application.Current.MainPage as NavigationPage;
            await navigationPage.PushAsync(new ItemDetailView(item)); // HERE
        }

        async void OnItemSelected(int id)
        {
            bool confirm = await DisplayAlert("Alerta",
                                      "Está seguro que desea eliminar la información de este cliente.",
                                      "Aceptar",
                                      "Cancelar");

            if (confirm)
            {
                await App.Database.DeleteUsersAsync(id);

                await DisplayAlert("Alerta",
                                   "Se ha eliminado el registro con éxito.",
                                   "Aceptar");

                await ExecuteLoadItemsCommand();
            }
        }

        private async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return await Application.Current.MainPage.DisplayAlert(title,
                                                            message,
                                                            accept,
                                                            cancel);
        }

        private async Task DisplayAlert(string title, string message, string cancel)
        {
            await Application.Current.MainPage.DisplayAlert(title,
                                                            message,
                                                            cancel);
        }
    }
}