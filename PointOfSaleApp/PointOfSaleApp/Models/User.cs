﻿namespace PointOfSaleApp.Models
{
    using SQLite;
    public class User
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
    }
}
