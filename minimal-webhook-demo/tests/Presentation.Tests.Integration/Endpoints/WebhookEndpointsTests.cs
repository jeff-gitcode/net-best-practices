namespace minimal_webhook_demo.Presentation.Tests.Integration.Endpoints;

using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class FakewebhookReceiver : IReceiveWebhook
{
    public List<string> Receipts = new List<string>();

    public async Task<string> ProcessRequest(string requestBody)
    {
        Receipts.Add(requestBody);

        return "Hello back";
    }
}


public class WebhookEndpointsTests
{
    private async Task WithTestServer(
    Func<HttpClient, IServiceProvider, Task> test,
    Action<IServiceCollection> configureServices)
    {
        await using var application =
            new WebApplicationFactory<Program>().
            WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => configureServices(services));
            });

        using var client = application.CreateClient();
        await test(client, application.Services);
    }


    [Fact]
    public async Task TestReceivingWebhook()
    {
        var fakeReceiver = new FakewebhookReceiver();

        await WithTestServer(async (c, s) =>
        {
            var response = await c.PostAsync("/api/webhook?requestBody=abc", null);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseText = await response.Content.ReadAsStringAsync();

            //Verify we got the expected response
            Assert.Equal("Hello back", responseText);

            //Verify that we received the correct details from the webhook
            Assert.Equal("Hi", fakeReceiver.Receipts.First());

        }, s => s.AddSingleton((IReceiveWebhook)fakeReceiver));
    }


    [Fact]
    public async Task TestLiveWebhook()
    {
        var client = new HttpClient();
        var response = await client.PostAsync("https://localhost:7032/api/webhook?requestBody=abc", new StringContent(""));
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("{\"message\" : \"Thanks! We got your webhook\"}", responseBody);
    }
}
