namespace PointOfSaleApp.Views
{
    using PointOfSaleApp.ViewModels;
    using Xamarin.Forms;
    public partial class NewItemView : ContentPage
    {
        public NewItemView()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}