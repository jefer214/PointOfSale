namespace PointOfSaleApp.ViewModels
{
    using PointOfSaleApp.Models;
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class SalesHistoryViewModel : BaseViewModel
    {
        public ObservableCollection<Billing> Billing { get; }
        public Command LoadItemsCommand { get; }

        public ICommand PerformSearch => new Command<string>((string query) =>
        {
            int.TryParse(query, out int result);
            FilterDataCommand(result);
        });
        public SalesHistoryViewModel()
        {
            Title = "Histórico de ventas";
            Billing = new ObservableCollection<Billing>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }
        public void OnAppearing()
        {
            IsBusy = true;
        }

        public async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Billing.Clear();

                var billing = await App.Database.GetInfoBillingAsync();

                foreach (var item in billing)
                {
                    Billing.Add(item);
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

        public void RefreshHistory()
        {
            IsBusy = true;
        }
        
        private void FilterDataCommand(int invoiceNumber)
        {
            var billing = Billing.Where(b => b.InvoiceNumber == invoiceNumber).ToList();
            Billing.Clear();

            foreach (Billing item in billing)
            {
                Billing.Add(item);
            }
        }
    }
}
