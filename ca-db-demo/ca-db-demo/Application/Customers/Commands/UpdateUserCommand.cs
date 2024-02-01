using Application.Abstraction;
using Domain;

public record UpdateUserCommand(CustomerModel Customer) : ICommand<CustomerModel>;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, CustomerModel>
{
    private readonly IUserRepository _repository;

    public UpdateUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomerModel> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.Update(request.Customer);

        return user;
    }
}