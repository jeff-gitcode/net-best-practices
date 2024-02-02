using Domain;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace BrewUp.ContractTest.Consumers;

public class Client
{
    private readonly HttpClientFixture _clientFixture;

    public Client(HttpClientFixture clientFixture)
    {
        _clientFixture = clientFixture;
    }

    public async Task<HttpResponseMessage> CreateOrder()
    {
        using var client = new HttpClient();

        var _baseUri = new Uri("http://localhost:5293");
        var purchaseOrder = new CustomerModel()
        {
            Id = "1",
            Name = "Test",
            Email = "test@test.com",
            Password = "password",
            Token = "123",

        };
            
        //    new Modules.Purchases.BindingModels.Order
        //{
        //    SupplierId = Guid.NewGuid(),
        //    Date = DateTime.UtcNow,
        //    Lines = new[]
        //    {
        //        new Modules.Purchases.BindingModels.OrderLine
        //        {
        //            ProductId = Guid.NewGuid(),
        //            Quantity = new Modules.Purchases.BindingModels.Quantity(10, "Nr"),
        //            Price = new Modules.Purchases.BindingModels.Price(10, "EUR")
        //        }
        //    }
        //};
        var stringJson = JsonSerializer.Serialize(purchaseOrder);
        var httpContent = new StringContent(stringJson, Encoding.UTF8, "application/json");

        return await client.PostAsync(new Uri(_baseUri, "/api/Customer"), httpContent);
    }

    public async Task<HttpResponseMessage> GetOrders()
    {
        using var client = new HttpClient();

        var _baseUri = new Uri("http://localhost:5293");
        client.BaseAddress = _baseUri;

       var stringJson = JsonSerializer.Serialize("");
        var httpContent = new StringContent(stringJson, Encoding.UTF8, "application/json");

        return await client.PostAsync(new Uri(_baseUri, "/api/Customer"), httpContent);
    }
}