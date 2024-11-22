namespace PointOfSaleApp
{
    using PointOfSaleApp.Services;
    using PointOfSaleApp.Views;
    using System;
    using System.IO;
    using Xamarin.Forms;
    public partial class App : Application
    {
        private static Data.Database database;

        public static Data.Database Database
        {
            get
            {
                if (database == null)
                {
                    database = new Data.Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "pointOfSale.db3"));
                }

                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<INavigationService, NavigationService>();
            MainPage = new NavigationPage(new LoginView());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
