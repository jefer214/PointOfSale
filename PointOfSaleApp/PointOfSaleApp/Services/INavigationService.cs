namespace PointOfSaleApp.Services
{
    using System.Threading.Tasks;

    public interface INavigationService
    {
        Task NavigateToAsync<TViewModel>();
    }
}
