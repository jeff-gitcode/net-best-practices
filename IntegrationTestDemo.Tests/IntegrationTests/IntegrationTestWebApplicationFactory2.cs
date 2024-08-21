using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace IntegrationTestDemo.Tests.IntegrationTests;

public class IntegrationTestWebApplicationFactory2<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

    public IntegrationTestWebApplicationFactory2()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((host, configurationBuilder) =>
        {
        });

        builder.ConfigureTestServices(ConfigureTestServices);
    }

    protected virtual void ConfigureTestServices(IServiceCollection services)
    {
        services.RemoveAll<HttpClient>();

        services.AddSingleton<HttpClient>(new HttpClient(_mockHttpMessageHandler.Object));
    }

    //public void StubHttpRequest<T>(string requestUrl, HttpStatusCode statusCode, T content)
    //{
    //    StubHttpRequest(requestUrl, statusCode, JsonSerializer.Serialize(content));
    //}

    public void StubHttpRequest<T>(string requestUrl, HttpStatusCode statusCode, T content)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            // etc.
        };

        var stringContent = JsonSerializer.Serialize(content, options);
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg => msg.RequestUri!.ToString().EndsWith(requestUrl, StringComparison.InvariantCultureIgnoreCase)),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(stringContent),
            });
    }

    //public void SetupMockHttpMessageHandlerResponse(HttpResponseMessage httpResponseMessage)
    //{
    //    _mockHttpMessageHandler.Setup(h => h.Send(It.IsAny<HttpRequestMessage>())).Returns(httpResponseMessage);
    //}
}

