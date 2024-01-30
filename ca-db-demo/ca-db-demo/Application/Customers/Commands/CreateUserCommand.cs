using Application.Abstraction;
using Domain;
using MediatR;

namespace Application.Users.Commands
{
    public record CreateUserCommand(Customer user) : ICommand<Customer> { }

    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Customer>
    {
        private readonly IUserRepository _repository;
        private readonly IPublisher _publisher;

        public CreateUserCommandHandler(IUserRepository repository, IPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        public async Task<Customer> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var result = await _repository.Add(request.user);

            // await _publisher.Publish(new CreateUserNotification(result), cancellationToken);

            return result;
        }
    }
}
