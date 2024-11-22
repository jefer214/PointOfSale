using System.Linq;

namespace PointOfSaleApp.Services
{
    using Newtonsoft.Json;
    using PointOfSaleApp.Entities;
    using PointOfSaleApp.Models;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    public class ApiService
    {
        public static class Cloud
        {
            public readonly static string ServerUrl = "https://fakestoreapi.com";
        }
        public async Task<List<Product>> GetInfoProductsAsync()
        {
            List<Product> products = new List<Product>();

            HttpClient _httpClient = new HttpClient();

            string url = $"{Cloud.ServerUrl}/products";

            HttpResponseMessage response = await _httpClient.GetAsync(url); 
                
            if (response.IsSuccessStatusCode)
            {
                string resultContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Product>>(resultContent);
            }

            return products;
        }

        public async Task<List<User>> GetInfoUsersAsync()
        {
            List<User> users = new List<User>();

            HttpClient _httpClient = new HttpClient();

            string url = $"{Cloud.ServerUrl}/users";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string resultContent = await response.Content.ReadAsStringAsync();
                List<UserResponse> userResponse = JsonConvert.DeserializeObject<List<UserResponse>>(resultContent);

                users.AddRange(from item in userResponse
                               let user = new User()
                               {
                                   FullName = $"{item.Name.Firstname} {item.Name.Lastname}",
                                   Email = item.Email,
                                   Id = item.Id,
                                   Phone = item.Phone,
                               }
                               select user);
            }

            return users;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            HttpClient _httpClient = new HttpClient();
            string url = $"{Cloud.ServerUrl}/auth/login";

            Dictionary<string, string> values = new Dictionary<string, string> { { "username", username }, { "password", password } };
            FormUrlEncodedContent content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string resultContent = await response.Content.ReadAsStringAsync();
                TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(resultContent);

                return tokenResponse.Token;
            }

            return string.Empty;
        }
    }
}
