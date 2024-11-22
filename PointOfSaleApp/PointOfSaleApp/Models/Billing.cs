namespace PointOfSaleApp.Models
{
    using SQLite;
    using System;

    public class Billing
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int InvoiceNumber { get; set; }
        public DateTime Date { get; set; }

        [Indexed]
        public int ProductId { get; set; }

        public string ProductName { get; set; }
        public int Quantity { get; set; }

        public double TotalPrice { get; set; }

        [Indexed]
        public int ClientId { get; set; }
        public string ClientName { get; set; }
    }
}
