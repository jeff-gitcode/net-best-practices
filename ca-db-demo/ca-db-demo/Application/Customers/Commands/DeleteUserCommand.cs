using Application.Abstraction;
using Domain;

namespace Application.Users.Commands
{
    public record DeleteUserCommand(string id) : ICommand<CustomerModel> { }

    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, CustomerModel>
    {
        public Task<CustomerModel> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
