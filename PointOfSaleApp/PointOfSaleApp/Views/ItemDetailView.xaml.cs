namespace PointOfSaleApp.Views
{
    using PointOfSaleApp.Models;
    using PointOfSaleApp.ViewModels;
    using Xamarin.Forms;
    public partial class ItemDetailView : ContentPage
    {
        public ItemDetailView(ItemClient item)
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel(item);
        }
    }
}