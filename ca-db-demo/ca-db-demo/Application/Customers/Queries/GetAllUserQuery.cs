using Application.Abstraction;
using Domain;
using MediatR;

namespace Application.Users.Queries
{
    public class GetAllUserQuery : IQuery<IEnumerable<CustomerModel>> { }

    public class GetAllUserQueryHandler : IQueryHandler<GetAllUserQuery, IEnumerable<CustomerModel>>
    {
        private readonly IUserRepository _repository;
        private readonly IPublisher _publisher;

        public GetAllUserQueryHandler(IUserRepository repository, IPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        public async Task<IEnumerable<CustomerModel>> Handle(
            GetAllUserQuery request,
            CancellationToken cancellationToken
        )
        {
            var result = await _repository.GetAll();

            // await _publisher.Publish(new GetAllUserNotification(result), cancellationToken);

            return result;
        }
    }
}
