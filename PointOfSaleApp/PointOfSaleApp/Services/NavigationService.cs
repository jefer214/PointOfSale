namespace PointOfSaleApp.Services
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class NavigationService : INavigationService
    {
        public Task NavigateToAsync<TViewModel>()
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        private Page CreatePage(Type viewModelType, object parameter)
        {
            try
            {
                Type pageType = GetPageTypeForViewModel(viewModelType);

                return pageType is null
                    ? throw new Exception($"No se la podido navegar a la página {viewModelType}")
                    : Activator.CreateInstance(pageType) as Page;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        { 
            Page page = CreatePage(viewModelType, parameter);
            NavigationPage navigationPage = Application.Current.MainPage as NavigationPage;

            if (navigationPage != null)
            {
                await navigationPage.PushAsync(page);
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(page);
            }
        }

        public Type GetPageTypeForViewModel(Type viewModelType)
        {
            string viewName = viewModelType.FullName.Replace("Model", string.Empty);

            string viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;

            string viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);

            return Type.GetType(viewAssemblyName);
        }
    }
}
