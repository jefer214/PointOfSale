namespace PointOfSaleApp.Data
{
    using PointOfSaleApp.Models;
    using SQLite;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Database
    {
        private readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Product>();
            _database.CreateTableAsync<User>();
            _database.CreateTableAsync<Billing>();
        }
        public Task<List<Product>> GetProductsAsync()
        {
            return _database.Table<Product>().ToListAsync();
        }

        public Task<List<User>> GetUsersAsync()
        {
            return _database.Table<User>().ToListAsync();
        }

        public async Task<int> GetNextUserIdUAsync()
        {
            List<User> users = await _database.Table<User>().ToListAsync();
            return users.Max(us => us.Id) + 1;
        }

        public Task<int> DeleteUsersAsync(int id)
        {
            return _database.Table<User>().Where(u => u.Id == id).DeleteAsync();
        }

        public Task<List<Billing>> GetInfoBillingAsync()
        {
            return _database.Table<Billing>().ToListAsync();
        }

        public Task<int> SaveProductsAsync(List<Product> products)
        {
            return _database.InsertAllAsync(products);
        }

        public Task<int> SaveUserAsync(User user)
        {
            return _database.InsertAsync(user);
        }

        public Task<int> UpdateUserAsync(User user)
        {
            return _database.UpdateAsync(user);
        }

        public Task<int> SaveUserAsync(List<User> users)
        {
            return _database.InsertAllAsync(users);
        }

        public Task<int> SaveBillingAsync(List<Billing> billing)
        {
            return _database.InsertAllAsync(billing);
        }
    }
}
