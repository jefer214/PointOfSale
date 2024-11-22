namespace PointOfSaleApp.Models
{
    using SQLite;

    public class Product
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public string PriceCurrencySymbol { get; set; }
    }
}
