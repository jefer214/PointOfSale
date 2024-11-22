namespace PointOfSaleApp.ViewModels
{
    using PointOfSaleApp.Models;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class NewItemViewModel : BaseViewModel
    {
        private string _fullName;
        private string _email;
        private string _phone;

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(_fullName)
                && !string.IsNullOrWhiteSpace(_email);
        }

        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnSave()
        {
            int id = await App.Database.GetNextUserIdUAsync();

            User newUser = new User()
            {
                Id = id,
                FullName = FullName,
                Phone = Phone,
                Email = Email
            };

            await App.Database.SaveUserAsync(newUser);

            await DisplayAlert("Exitosa",
                                   "Información registrada de manera exitosa.",
                                   "Aceptar");

            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void OnCancel()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async Task DisplayAlert(string title, string message, string cancel)
        {
            await Application.Current.MainPage.DisplayAlert(title,
                                                            message,
                                                            cancel);
        }
    }
}
