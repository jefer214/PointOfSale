namespace PointOfSaleApp.Models
{
    public class ItemProduct
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string PriceCurrencySymbol { get; set; }
    }
}