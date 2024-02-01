using Application.Abstraction;
using Domain;
using MediatR;

public record GetUserQuery(string Id) : IQuery<CustomerModel>;

public class GetUserQueryHandler : IQueryHandler<GetUserQuery, CustomerModel>
{
    private readonly IUserRepository _repository;
    private readonly IPublisher _publisher;

    public GetUserQueryHandler(IUserRepository repository, IPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<CustomerModel> Handle(
        GetUserQuery request,
        CancellationToken cancellationToken
    )
    {
        var result = await _repository.Get(request.Id);

        // await _publisher.Publish(new GetUserNotification(result), cancellationToken);

        return result;
    }
}