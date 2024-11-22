namespace PointOfSaleApp.ViewModels
{
    using PointOfSaleApp.Models;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class ItemDetailViewModel : BaseViewModel
    {
        private ItemClient client;

        public Command UpdateCommand { get; }
        public Command CancelCommand { get; }

        public ItemClient Client
        {
            get => client;
            set => SetProperty(ref client, value);
        }

        public ItemDetailViewModel(ItemClient client)
        {
            Client = client;

            UpdateCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => UpdateCommand.ChangeCanExecute();
        }
        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(Client.FullName)
                && !string.IsNullOrWhiteSpace(Client.Email)
                && !string.IsNullOrWhiteSpace(Client.Phone);
        }

        private async void OnSave()
        {
            User newUser = new User()
            {
                Id = Client.Id,
                FullName = Client.FullName,
                Phone = Client.Phone,
                Email = Client.Email
            };

            await App.Database.UpdateUserAsync(newUser);

            await DisplayAlert("Exitosa",
                                   "Información actualizada de manera exitosa.",
                                   "Aceptar");

            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async Task DisplayAlert(string title, string message, string cancel)
        {
            await Application.Current.MainPage.DisplayAlert(title,
                                                            message,
                                                            cancel);
        }

        private async void OnCancel()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
