using Application.Abstraction;
using Domain;

namespace Application.Users.Commands
{
    public record DeleteUserCommand(string id) : ICommand<CustomerModel> { }

    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, CustomerModel>
    {
        private readonly IUserRepository _repository;

        public DeleteUserCommandHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<CustomerModel> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.Delete(request.id);

            return user;
        }
    }
}
