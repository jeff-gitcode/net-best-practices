namespace minimal_webhook_demo.Application.Versions.Queries.GetVersion;

using Entities;
using MediatR;

public class GetVersionQuery : IRequest<Version>
{
}
