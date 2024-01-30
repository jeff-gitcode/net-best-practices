using Application.Abstraction;
using Domain;

namespace Application.Users.Commands
{
    public record DeleteUserCommand(string id) : ICommand<Customer> { }

    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, Customer>
    {
        public Task<Customer> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
