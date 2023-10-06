using FluentValidation;
using MediatR;
using minimal_webhook_demo.Application.Common.Enums;
using minimal_webhook_demo.Application.Common.Exceptions;

public record GetWebhookQuery : IRequest<string>
{
    public string requestBody;
}

public class GetWebhookValidator : AbstractValidator<GetWebhookQuery>
{
    public GetWebhookValidator()
    {
        _ = this.RuleFor(r => r.requestBody).NotEmpty().NotNull().WithMessage("A webhook was not supplied.");
    }
}

public class GetWebhookHandler : IRequestHandler<GetWebhookQuery, string>
{
    private readonly IReceiveWebhook receiveWebhook;

    public GetWebhookHandler(IReceiveWebhook repository)
    {
        this.receiveWebhook = repository;
    }

    public async Task<string> Handle(GetWebhookQuery request, CancellationToken cancellationToken)
    {
        var result = await this.receiveWebhook.ProcessRequest(request.requestBody);

        NotFoundException.ThrowIfNull(result, EntityType.Webhook);

        return result;
    }
}


