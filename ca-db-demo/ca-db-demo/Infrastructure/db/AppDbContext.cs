namespace Infrastructure.db;

using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    {
    
    }

    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Customer>().HasData(
            new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = "John Doe",
                Email = "john.doe@test.com",
                Password = "test",
                Role = "",
                Token = ""
            }
        );
    }
}