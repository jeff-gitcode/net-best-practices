using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UnitTests
{
    public class ProviderResponse
    {
        public IEnumerable<string> Data { get; set; }
    }

    // Consumes Provider API
    public class ProviderService
    {
        private readonly HttpClient _client;

        public ProviderService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            if (_client.BaseAddress is null)
            {
                throw new ArgumentNullException(nameof(_client.BaseAddress));
            }
        }

        public async Task<ProviderResponse> GetData()
        {
            var httpResponse = await _client.GetAsync("/api/customer");
            if (httpResponse.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<ProviderResponse>(
                        await httpResponse.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            return new ProviderResponse() { Data = Enumerable.Empty<string>() };
        }
    }
}
