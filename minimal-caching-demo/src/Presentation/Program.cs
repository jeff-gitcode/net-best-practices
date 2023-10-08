using minimal_caching_demo.Presentation.Endpoints.Authors;
using minimal_caching_demo.Presentation.Endpoints.Caching;
using minimal_caching_demo.Presentation.Endpoints.Movies;
using minimal_caching_demo.Presentation.Endpoints.Reviews;
using minimal_caching_demo.Presentation.Endpoints.Version;
using minimal_caching_demo.Presentation.Extensions;
using Serilog;

var builder = WebApplication
                .CreateBuilder(args)
                .ConfigureBuilder();
var app = builder
            .Build()
            .ConfigureApplication();

_ = app.MapVersionEndpoints();
_ = app.MapAuthorEndpoints();
_ = app.MapMovieEndpoints();
_ = app.MapReviewEndpoints();
_ = app.MapCachingEndpoints();

try
{
    Log.Information("Starting host");
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
