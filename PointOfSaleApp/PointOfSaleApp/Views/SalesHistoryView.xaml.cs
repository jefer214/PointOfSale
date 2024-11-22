namespace PointOfSaleApp.Views
{
    using PointOfSaleApp.ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SalesHistoryView : ContentPage
    {
        SalesHistoryViewModel _viewModel;
        public SalesHistoryView()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SalesHistoryViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private async void SearchBarPatients_TextChanged(object sender, TextChangedEventArgs e)
        {
           if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                _viewModel.RefreshHistory();
            }
        }
    }
}