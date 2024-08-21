using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace IntegrationTestDemo.Tests.IntegrationTests;



public class IntegrationTestWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    public MockHttpMessageHandler MockHttpHandler { get; set; }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((host, configurationBuilder) =>
        {
        });

        builder.ConfigureTestServices(ConfigureTestServices);
    }

    protected virtual void ConfigureTestServices(IServiceCollection services)
    {
        //Add stub classes
        //_stub = Mock.Of<ISampleDependency>();
        //services.RemoveAll<ISampleDependency>();
        // services.TryAddTransient(_ => _stub);
        //or services.AddTransient<ISampleDependency, MockSampleDependency>();

        services.RemoveAll<HttpClient>();

        MockHttpHandler = new MockHttpMessageHandler();

        var httpClient = MockHttpHandler.ToHttpClient();

        services.AddSingleton<HttpClient>(httpClient);

        //Configure logging
        services.AddLogging(builder => builder.ClearProviders().AddConsole().AddDebug());
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            //var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TodoDbContext>));

            //if (descriptor != null)
            //{
            //    services.Remove(descriptor);
            //}

            //services.AddDbContext<TodoDbContext>(options =>
            //{
            //    //var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            //    //options.UseSqlite($"Data Source={Path.Join(path, "WebMinRouteGroup_tests.db")}");
            //    options.UseInMemoryDatabase($"InMemoryTestDb-{DateTime.Now.ToFileTimeUtc()}");
            //});

            //services.Replace(ServiceDescriptor.Scoped(_ =>
            //{
            //    var providerMock = new Mock<IXDataProvider>();
            //    providerMock.Setup(x => x.GetData()).Returns(new XData { Attr1 = "Val1", Attr2 = "Val2" });
            //    return providerMock.Object;
            //}));
        });


        return base.CreateHost(builder);
    }
}

public static class MockHttpClientBunitHelpers
{
    public static MockHttpMessageHandler AddMockHttpClient(this IServiceCollection services)
    {
        var mockHttpHandler = new MockHttpMessageHandler();
        var httpClient = mockHttpHandler.ToHttpClient();
        // httpClient.BaseAddress = new Uri("http://localhost");
        services.AddSingleton<HttpClient>(httpClient);
        return mockHttpHandler;
    }

    public static MockedRequest RespondJson<T>(this MockedRequest request, T content)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            // etc.
        };
        request.Respond(req =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(JsonSerializer.Serialize(content, options));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        });
        return request;
    }

    public static MockedRequest RespondJson<T>(this MockedRequest request, Func<T> contentProvider)
    {
        request.Respond(req =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(JsonSerializer.Serialize(contentProvider()));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        });
        return request;
    }
}