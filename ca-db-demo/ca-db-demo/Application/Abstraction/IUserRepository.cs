using Domain;
using MediatR;

namespace Application.Abstraction;

public interface IUserRepository : IRepository<Customer>
{
    // Task<List<Customer>> Search(Specification<Customer> specification, Pagination? pagination);
    // Task<User> CreateUser(User user);
    // User Get(int id);
    Task<Customer> GetByEmail(string email);
    IAsyncEnumerable<Customer> GetAll();
    Task<Customer> AddAsync(Customer tempUser);
    Task<Customer> UpdateAsync(Customer tempUser);
    Task<Unit> DeleteAsync(string id);
}
