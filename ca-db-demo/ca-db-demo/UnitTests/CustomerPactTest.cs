using PactNet;
using Xunit.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PactNet.Output.Xunit;
using PactNet.Infrastructure.Outputters;
using Domain;

namespace tests
{

    public class ConsumerPactTests2
    {
        private readonly IPactBuilderV3 pact;

        public ConsumerPactTests2(ITestOutputHelper output)
        {
            var config = new PactConfig
            {
                PactDir = "../../../pacts/",
                LogLevel = PactLogLevel.Debug,
                Outputters = new List<IOutput> { new XunitOutput(output), new ConsoleOutput() },

                DefaultJsonSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            };

            String provider = Environment.GetEnvironmentVariable("PACT_PROVIDER");
            // you select which specification version you wish to use by calling either V2 or V3
            IPactV3 pact = Pact.V3("contract-test", provider != null ? provider : "contract-test", config);

            // the pact builder is created in the constructor so it's unique to each test
            this.pact = pact.WithHttpInteractions();
        }

        [Fact]
        public async Task GetCustomers_WhenCalled_ReturnsAllCustomers()
        {
            var expected = new List<CustomerModel> {
               new CustomerModel()
            {
                Id = "1",
                Name = "Test",
                Email = "test@test.com",
                Password = "password",
                Token = "123",

            } };

            //var stringJson = JsonSerializer.Serialize(customer);
            //var httpContent = new StringContent(stringJson, Encoding.UTF8, "application/json");

            //Arrange
            pact
                .UponReceiving("a request to retrieve all customers")
                .WithRequest(HttpMethod.Get, "/api/Customer")
                .WillRespond()
                .WithStatus(System.Net.HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json; charset=utf-8")
                .WithJsonBody(expected);

            //Act
            await pact.VerifyAsync(async ctx =>
            {
                var client = new TestClient();
                var result = await client.GetCustomers(ctx.MockServerUri.AbsoluteUri, null);



                Assert.Equal(expected, result);

                //Assert
                //Assert.IsType<int>(customers[0].Id);
                //Assert.IsType<string>(customers[0].Name);
                //Assert.IsType<string>(customers[0].Email);
            });
            //the mock server is no longer running once VerifyAsync returns
        }

        [Fact]
        public async Task GetCustomer_WhenCalledWithExistingId_ReturnsCustomer()
        {
            var expected = new
                CustomerModel
            {
                Id = "1",
                Name = "burger",
                Email = "food@test.com"
            };

            pact
                .UponReceiving("a request to retrieve a customer with existing id")
                .WithRequest(HttpMethod.Get, "/api/Customer/1")
                .WillRespond()
                .WithStatus(System.Net.HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json; charset=utf-8")
                .WithJsonBody(expected);

            //Act
            await pact.VerifyAsync(async ctx =>
            {
                var client = new TestClient();
                var result = await client.GetById(ctx.MockServerUri.AbsoluteUri, 1, null);

                Assert.Equal(expected, result);
            });
        }

        [Fact]
        public async Task GetCustomer_WhenCalledWithInvalidID_ReturnsError()
        {
            pact
                .UponReceiving("a request to retrieve a id that does not exist")
                .WithRequest(HttpMethod.Get, "/api/Customer/20")
                .WillRespond()
                .WithStatus(System.Net.HttpStatusCode.NotFound)
                .WithHeader("Content-Type", "application/json; charset=utf-8");


            //Act
            await pact.VerifyAsync(async ctx =>
            {
                var client = new TestClient();

                //Assert
                var ex = await Assert.ThrowsAsync<HttpRequestException>(() => client.GetById(ctx.MockServerUri.AbsoluteUri, 20, null));
                Assert.Equal("Response status code does not indicate success: 404 (Not Found).", ex.Message);   
            });
        }
    }
}