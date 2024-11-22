namespace PointOfSaleApp.ViewModels
{
    using PointOfSaleApp.Models;
    using PointOfSaleApp.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class BillingViewModel : BaseViewModel
    {
        readonly ApiService apiService = new ApiService();
        public Command SaveCommand { get; }
        ObservableCollection<Product> _products;
        private List<User> _listClients;
        private User _selectedClient;
        private string _infoProductos;
        private string _total;

        public ObservableCollection<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        public List<User> ListClients
        {
            get => _listClients;
            set => SetProperty(ref _listClients, value);
        }

        public User SelectedClient
        {
            get => _selectedClient;
            set => SetProperty(ref _selectedClient, value);
        }

        public string InfoProductos
        {
            get => _infoProductos;
            set => SetProperty(ref _infoProductos, value);
        }

        public string Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }

        public BillingViewModel()
        {
            SaveCommand = new Command(OnSaveClicked, ValidateLogin);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            Title = "Facturación";
            Products = new ObservableCollection<Product>();
            ListClients = Task.Run(() => GetClients()).Result;
            GetProducts();
        }

        private async Task<List<User>> GetClients()
        {
            List<User> users = await App.Database.GetUsersAsync();

            if (!users.Any())
            {
                users = await apiService.GetInfoUsersAsync();
            }

            return users;
        }

        private async Task GetProducts()
        {
            List<Product> products = await App.Database.GetProductsAsync();

            if (!products.Any())
            {
                products = await apiService.GetInfoProductsAsync();
            }

            foreach (Product product in products)
            {
                product.PriceCurrencySymbol = $"$ {product.Price}";
                Products.Add(product);
            }
        }

        public void OnAppearing()
        {
            InfoProductos = "No hay productos seleccionados";
            Total = "Total: ";
        }

        private bool ValidateLogin()
        {
            return SelectedClient != null
                && Products.Any(p => p.Quantity > 0);
        }

        public void UpdateInfoProducts()
        {
            var numberProductsSelected = Products.Sum(p => p.Quantity);

            if (numberProductsSelected == 0)
            {
                InfoProductos = "No hay productos seleccionados";
                Total = "Total: ";
            }
            else
            {
                double sumPrice = 0;
                List<Product> productsSelected = Products.Where(p => p.Quantity > 0).ToList();
                
                foreach (double price in from Product product in productsSelected
                                      let price = product.Price * product.Quantity
                                      select price)
                {
                    sumPrice += price;
                }

                InfoProductos = $"Productos seleccionados ({numberProductsSelected})";
                Total = $"Total: ${sumPrice}";
            }
        }

        private async void OnSaveClicked()
        {
            if (SelectedClient is null)
            {
                await DisplayAlert("Error",
                                   "Debe seleccionar un cliente.",
                                   "Accept");
                return;
            }

            List<Product> productsSelected = Products.Where(p => p.Quantity > 0).ToList();
            List<Billing> elementsBilling = new List<Billing>();

            if (!productsSelected.Any())
            {
                await DisplayAlert("Error",
                                   "Debe seleccionar productos para realizar la facturación.",
                                   "Accept");
                return;
            }

            Random rnd = new Random();
            int invoiceNumber = rnd.Next(1, 1000);

            foreach (var product in productsSelected)
            {
                Billing billing = new Billing()
                { 
                    Date = DateTime.Now,
                    InvoiceNumber = invoiceNumber,
                    ClientId = SelectedClient.Id,
                    ClientName = SelectedClient.FullName,
                    ProductId = product.Id,
                    ProductName = product.Title,
                    Quantity = product.Quantity,
                    TotalPrice = product.Price * product.Quantity
                };

                elementsBilling.Add(billing);
            }

            await App.Database.SaveBillingAsync(elementsBilling);

            await DisplayAlert("Exitosa",
                                   "Información registrada de manera exitosa.",
                                   "Aceptar");

            SelectedClient = null;
            for (int i = 0; i < Products.Count; i++)
            {
                Product item = Products[i];
                item.Quantity = 0;
            }
        }

        private async Task DisplayAlert(string title, string message, string cancel)
        {
            await Application.Current.MainPage.DisplayAlert(title,
                                                            message,
                                                            cancel);
        }
    }
}
