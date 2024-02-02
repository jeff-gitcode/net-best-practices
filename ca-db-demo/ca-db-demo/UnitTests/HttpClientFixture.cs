using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace BrewUp.ContractTest;

public class HttpClientFixture : IDisposable
{
    public readonly HttpClient Client;

    public HttpClientFixture()
    {
        var app = new ProjectsApplication();
        Client = app.CreateClient();
        Client.BaseAddress = new Uri("http://localhost:5293/");
    }

    private class ProjectsApplication : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureHostConfiguration(_ =>
            {
            });

            return base.CreateHost(builder);
        }
    }

    #region Dispose

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        Client.Dispose();
    }

    #endregion
}