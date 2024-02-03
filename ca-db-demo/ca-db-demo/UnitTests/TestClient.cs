using Newtonsoft.Json;
using Domain;
using System.Text;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace tests
{
    public class TestClient
    {
#nullable enable
        public async Task<List<CustomerModel>> GetCustomers(string baseUrl, HttpClient? httpClient = null)
        {
            using var client = httpClient == null ? new HttpClient() : httpClient;

            var response = await client.GetAsync(baseUrl + "api/Customer");
            response.EnsureSuccessStatusCode();

            var responseStr = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<CustomerModel>>(responseStr);
        }


        public async Task<CustomerModel> GetById(string baseUrl, int id, HttpClient? httpClient = null)
        {
            using var client = httpClient == null ? new HttpClient() : httpClient;

            var response = await client.GetAsync(baseUrl + "api/Customer/" + id);
            response.EnsureSuccessStatusCode();

            var resp = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<CustomerModel>(resp);
        }

        public async Task<CustomerModel> Add(string baseUrl, CustomerModel item, HttpClient? httpClient = null)
        {
            using var client = httpClient == null ? new HttpClient() : httpClient;

            var stringJson = JsonSerializer.Serialize(item);
            var httpContent = new StringContent(stringJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(baseUrl + "api/Customer/", httpContent);
            response.EnsureSuccessStatusCode();

            var resp = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<CustomerModel>(resp);
        }

        public async Task<CustomerModel> Update(string baseUrl, CustomerModel item, HttpClient? httpClient = null)
        {
            using var client = httpClient == null ? new HttpClient() : httpClient;

            var stringJson = JsonSerializer.Serialize(item);
            var httpContent = new StringContent(stringJson, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(baseUrl + "api/Customer/", httpContent);
            response.EnsureSuccessStatusCode();

            var resp = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<CustomerModel>(resp);
        }

        public async Task Delete(string baseUrl, int id, HttpClient? httpClient = null)
        {
            using var client = httpClient == null ? new HttpClient() : httpClient;

            var item = new { id = id };

            var stringJson = JsonSerializer.Serialize(item);
            var httpContent = new StringContent(stringJson, Encoding.UTF8, "application/json");

            var response = await client.DeleteAsync(baseUrl + "api/Customer/" + id);
            response.EnsureSuccessStatusCode();
        }
    }
}