using clean_minimal_api_demo.Presentation.Endpoints.Authors;
using clean_minimal_api_demo.Presentation.Endpoints.Movies;
using clean_minimal_api_demo.Presentation.Endpoints.Reviews;
using clean_minimal_api_demo.Presentation.Endpoints.Version;
using clean_minimal_api_demo.Presentation.Extensions;
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

// Generate a method with date format 04/11/2019 03:18:22 am




