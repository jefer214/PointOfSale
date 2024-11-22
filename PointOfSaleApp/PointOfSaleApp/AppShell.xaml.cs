namespace PointOfSaleApp
{
    using PointOfSaleApp.Views;
    using Xamarin.Forms;
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailView), typeof(ItemDetailView));
            Routing.RegisterRoute(nameof(NewItemView), typeof(NewItemView));
        }

    }
}
