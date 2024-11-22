namespace PointOfSaleApp.ViewModels
{
    using PointOfSaleApp.Services;
    using System.Threading.Tasks;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class LoginViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }
        readonly ApiService apiService = new ApiService();
        public Command LoginCommand { get; }

        string _userName;
        string _password;


        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public LoginViewModel(INavigation navigation)
        {
            Navigation = navigation;
            LoginCommand = new Command(OnLoginClicked, ValidateLogin);
            this.PropertyChanged +=
                (_, __) => LoginCommand.ChangeCanExecute();
        }

        private bool ValidateLogin()
        {
            return !string.IsNullOrWhiteSpace(UserName)
                && !string.IsNullOrWhiteSpace(Password);
        }

        private async void OnLoginClicked()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                await DisplayAlert("Error",
                                   "Debe ingresar un correo electrónico.",
                                   "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await DisplayAlert("Error",
                                   "Debe ingresar la contraseña.",
                                   "Aceptar");
                return;
            }

            IsBusy = true;
            NetworkAccess current = Connectivity.NetworkAccess;

            if (current != NetworkAccess.Internet)
            {
                await DisplayAlert("Error",
                                   "¡Asegúrese de que su dispositivo esté conectado a Internet!",
                                   "Aceptar");

                return;
            }

            string responseLogin = await apiService.LoginAsync(UserName, Password);

            if (!string.IsNullOrEmpty(responseLogin))
            {
                await Navigation.PushAsync(new NavigationPage(new AppShell()));
            }
            else
            {
                await DisplayAlert("Error",
                                   "Usuario o contraseña incorrecta, intenta nuevamente.",
                                   "Aceptar");
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
