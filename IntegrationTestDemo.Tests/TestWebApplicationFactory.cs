using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

public class TestWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
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
        });

        return base.CreateHost(builder);
    }
}