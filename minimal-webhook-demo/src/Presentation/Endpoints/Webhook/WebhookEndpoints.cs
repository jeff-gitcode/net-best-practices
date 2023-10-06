namespace minimal_webhook_demo.Presentation.Endpoints.Webhook;

using MediatR;
using minimal_webhook_demo.Presentation.Errors;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

public static class WebhookEndpoints
{
    public static WebApplication MapWebhookEndpoints(this WebApplication app)
    {
        _ = app.MapPost("/api/webhook",
            async (IMediator mediator, string requestBody) =>
                Results.Ok(await mediator.Send(new GetWebhookQuery()
                {
                    requestBody = requestBody
                })))
        .WithTags("Webhook")
        .WithMetadata(new SwaggerOperationAttribute("Lookup Webhook", "\n    Post /Webhook"))
        .Produces<string>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
        .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);

        return app;
    }
}
