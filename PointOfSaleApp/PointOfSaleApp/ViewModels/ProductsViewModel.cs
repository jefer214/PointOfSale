namespace PointOfSaleApp.ViewModels
{
    using PointOfSaleApp.Models;
    using PointOfSaleApp.Services;
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Xamarin.Forms;
    public class ProductsViewModel : BaseViewModel
    {
        readonly ApiService apiService = new ApiService();
        public ObservableCollection<ItemProduct> Products { get; }
        public Command LoadItemsCommand { get; }

        public ProductsViewModel()
        {
            Title = "Productos";
            Products = new ObservableCollection<ItemProduct>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Products.Clear();

                System.Collections.Generic.List<Product> products = await App.Database.GetProductsAsync();

                if (!products.Any())
                {
                    products = await apiService.GetInfoProductsAsync();

                    await App.Database.SaveProductsAsync(products);
                }

                foreach (var i in from Product product in products
                                  let i = new ItemProduct()
                                  {
                                      Id = product.Id,
                                      Title = product.Title,
                                      Price = product.Price,
                                      PriceCurrencySymbol = $"$ {product.Price}"
                                  }
                                  select i)
                {
                    Products.Add(i);
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
        }
    }
}