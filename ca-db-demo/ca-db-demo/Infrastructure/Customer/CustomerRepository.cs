using Application.Abstraction;
using Domain;

public class CustomerRepository : IUserRepository
{
    public CustomerRepository()
    {
    }

    public Task<CustomerModel> Add(CustomerModel entity)
    {
        throw new NotImplementedException();
    }

    public Task<CustomerModel> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<CustomerModel> Get(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CustomerModel>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<CustomerModel> Update(CustomerModel entity)
    {
        throw new NotImplementedException();
    }

}