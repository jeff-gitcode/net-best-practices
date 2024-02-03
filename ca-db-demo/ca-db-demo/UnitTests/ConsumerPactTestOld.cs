using Domain;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PactNet;
using PactNet.Output.Xunit;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BrewUp.ContractTest;

public static class ConsumerApiClient
{
    static public async Task<HttpResponseMessage> GetPersonUsingGuideApi(
        Uri baseUri,
        CancellationToken cancellationToken = default)
    {
        using (var client = new HttpClient { BaseAddress = baseUri })
        {
            try
            {
                return await client.GetAsync($"/api/Customer", cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception("There was a problem connecting to Provider API.", ex);
            }
        }
    }
}

/// <summary>
/// https://github.com/DiUS/pact-workshop-dotnet-core-v3/
/// </summary>
public class PurchasesContracts
{
    private readonly IPactBuilderV3 _pactBuilder;
    private readonly Mock<IHttpClientFactory> _mockFactory;

    public PurchasesContracts(ITestOutputHelper output)
    {
        _mockFactory = new Mock<IHttpClientFactory>();

        var config = new PactConfig
        {
            PactDir = "../../../pacts/",
            Outputters = new[]
            {
                new XunitOutput(output)
            },
            DefaultJsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new JsonConverter[] { new StringEnumConverter() }
            },
            LogLevel = PactLogLevel.Debug
        };

        _pactBuilder = Pact.V3("BrewUp API", "Purchases Module", config).WithHttpInteractions();
    }

    [Fact]
    public async Task Should_Get_PurchasesOrders()
    {
        var purchaseOrder = new CustomerModel()
        {
            Id = "1",
            Name = "Test",
            Email = "test@test.com",
            Password = "password",
            Token = "123",

        };

        var stringJson = JsonSerializer.Serialize(purchaseOrder);
        var httpContent = new StringContent(stringJson, Encoding.UTF8, "application/json");

        _pactBuilder
            .UponReceiving("a request for purchases orders")
            .WithRequest(HttpMethod.Get, "/api/Customer")
            .WithHeader("Accept", "application/json")
            .WillRespond()
            .WithStatus(HttpStatusCode.OK)
            .WithHeader("Content-Type", "application/json; charset=utf-8")
            .WithJsonBody(httpContent);

        await _pactBuilder.VerifyAsync(async ctx =>
        {
            var response = await ConsumerApiClient.GetPersonUsingGuideApi(ctx.MockServerUri);
            var body = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            //_mockFactory
            //    .Setup(f => f.CreateClient("Purchases"))
            //    .Returns(() => new HttpClient
            //    {
            //        BaseAddress = ctx.MockServerUri,
            //        DefaultRequestHeaders =
            //        {
            //            Accept = { MediaTypeWithQualityHeaderValue.Parse("application/json") }
            //        }
            //    });


        });
    }
}