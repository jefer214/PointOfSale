namespace PointOfSaleApp.Entities
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Name Name { get; set; }
        public string Phone { get; set; }
    }
}
