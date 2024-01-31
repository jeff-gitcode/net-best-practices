using Application.Abstraction;
using Domain;

public class CustomerRepository : IUserRepository
{
    public CustomerRepository()
    {
    }

    public Task<Customer> Add(Customer entity)
    {
        throw new NotImplementedException();
    }

    public Task<Customer> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Customer> Get(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Customer>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Customer> Update(Customer entity)
    {
        throw new NotImplementedException();
    }
}