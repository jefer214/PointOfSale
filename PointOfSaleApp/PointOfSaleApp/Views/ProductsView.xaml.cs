namespace PointOfSaleApp.Views
{
    using PointOfSaleApp.ViewModels;
    using Xamarin.Forms;

    public partial class ProductsView : ContentPage
    {
        ProductsViewModel _viewModel;
        public ProductsView()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ProductsViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}