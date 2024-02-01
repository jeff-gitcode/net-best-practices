using Application.Abstraction;
using Domain;
using Infrastructure.db;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

public class CustomerRepository : IUserRepository
{
    protected readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<CustomerModel> Add(CustomerModel entity)
    {
        var user = new Customer()
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
            Password = entity.Password,
            Token = entity.Token,
        };

        _context.Customers.AddAsync(user);
        return Task.FromResult(entity);

    }

    public Task<CustomerModel> Delete(string id)
    {
        var user = _context.Customers.Find(id);
        _context.Customers.Remove(user);
        return Task.FromResult(new CustomerModel
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
            Token = user.Token,
        });
    }

    public Task<CustomerModel> Get(string id)
    {
        var user = _context.Customers.Find(id);
        return Task.FromResult(new CustomerModel
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
            Token = user.Token,
        });
    }

    public async Task<IEnumerable<CustomerModel>> GetAll()
    {
        await Task.CompletedTask;

        return _context.Customers.Select(x => new CustomerModel
        {
            Id = x.Id,
            Name = x.Name,
            Email = x.Email,
            Password = x.Password,
            Token = x.Token,
        });
    }

    public Task<CustomerModel> Update(CustomerModel entity)
    {
        var user = _context.Customers.Find(entity.Id);
        user.Name = entity.Name;
        user.Email = entity.Email;
        user.Password = entity.Password;
        user.Token = entity.Token;

        _context.Customers.Update(user);
        return Task.FromResult(entity);
    }

}