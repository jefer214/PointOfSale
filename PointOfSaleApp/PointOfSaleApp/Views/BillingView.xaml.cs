namespace PointOfSaleApp.Views
{
    using PointOfSaleApp.ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BillingView : ContentPage
    {
        BillingViewModel _viewModel;
        public BillingView()
        {
            InitializeComponent();
            BindingContext = _viewModel = new BillingViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
        private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            _viewModel.UpdateInfoProducts();
        }
    }
}