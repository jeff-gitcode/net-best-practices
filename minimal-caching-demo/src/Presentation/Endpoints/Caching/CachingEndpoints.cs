namespace minimal_caching_demo.Presentation.Endpoints.Caching;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using minimal_caching_demo.Application.Caching;
using minimal_caching_demo.Presentation.Errors;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;

[ExcludeFromCodeCoverage]
public static class CachingEndpoints
{
    public static WebApplication MapCachingEndpoints(this WebApplication app)
    {
        _ = app.MapGet("/api/caching",
                async (IMediator mediator, [FromQuery] string name) =>
                    Results.Ok(await mediator.Send(new GetDataQuery()
                    {
                        Name = name
                    })))
            .WithTags("caching")
            .WithMetadata(new SwaggerOperationAttribute("Lookup all Caching", "\n    GET /Caching"))
            .Produces<IList<DataEntity>>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

        return app;
    }
}
