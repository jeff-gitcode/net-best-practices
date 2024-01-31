using Domain;
using MediatR;

namespace Application.Abstraction;

public interface IUserRepository : IRepository<CustomerModel>
{

}
