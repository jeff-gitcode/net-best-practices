using IntegrationTestDemo.User;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
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
    private UserDbContext _userDbContext;
    private IDataSeedService _dataSeedService;
    private IServiceProvider _serviceProvider;

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
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<UserDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<UserDbContext>(options =>
            {
                //var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                //options.UseSqlite($"Data Source={Path.Join(path, "WebMinRouteGroup_tests.db")}");
                options.UseInMemoryDatabase($"InMemoryTestDb-{DateTime.Now.ToFileTimeUtc()}");
            });

            services.AddTransient<IDataSeedService, DataSeedService>();

            //services.Replace(ServiceDescriptor.Scoped(_ =>
            //{
            //    var providerMock = new Mock<IXDataProvider>();
            //    providerMock.Setup(x => x.GetData()).Returns(new XData { Attr1 = "Val1", Attr2 = "Val2" });
            //    return providerMock.Object;
            //}));

            _serviceProvider = services.BuildServiceProvider();
            _userDbContext = _serviceProvider.GetRequiredService<UserDbContext>();
            _dataSeedService = _serviceProvider.GetRequiredService<IDataSeedService>();

            _dataSeedService.SeedDataAsync().Wait();

            // UserDb = sp.serv
            //using (var scope = sp.CreateScope())
            //using (var appContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>())
            //{
            //    try
            //    {
            //        appContext.Database.EnsureCreated();
            //    }
            //    catch (Exception ex)
            //    {
            //        //Log errors or do anything you think it's needed
            //        throw;
            //    }
            //}
        });


        return base.CreateHost(builder);
    }

    public UserDbContext GetContext() => _userDbContext;

    public T GetService<T>() => _serviceProvider.GetRequiredService<T>();
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