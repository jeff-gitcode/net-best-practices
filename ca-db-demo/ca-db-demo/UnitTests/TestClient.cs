using Newtonsoft.Json;
using Domain;

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
    }
}   