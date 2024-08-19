using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class IntegrationTestWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((host, configurationBuilder) =>
        {
        });

        builder.ConfigureTestServices(services =>
        {
            //Add stub classes
            //_stub = Mock.Of<ISampleDependency>();
            //services.RemoveAll<ISampleDependency>();
            // services.TryAddTransient(_ => _stub);
            //or services.AddTransient<ISampleDependency, MockSampleDependency>();

            //Configure logging
            services.AddLogging(builder => builder.ClearProviders().AddConsole().AddDebug());
        });
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