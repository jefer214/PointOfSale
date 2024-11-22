namespace PointOfSaleApp.Views
{
    using PointOfSaleApp.ViewModels;
    using Xamarin.Forms;
    public partial class ClientsView : ContentPage
    {
        ClientsViewModel _viewModel;

        public ClientsView()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ClientsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}