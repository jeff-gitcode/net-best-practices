using Microsoft.EntityFrameworkCore;

namespace sieve_demo.Controllers;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Product 1",
                Price = 10,
                Stock = 1
            },
            new Product
            {
                Id = 2,
                Name = "Product 2",
                Price = 20,
                Stock = 2
            },
            new Product
            {
                Id = 3,
                Name = "Product 3",
                Price = 30,
                Stock = 3
            },
            new Product
            {
                Id = 4,
                Name = "Product 4",
                Price = 40,
                Stock = 4
            },
            new Product
            {
                Id = 5,
                Name = "Product 5",
                Price = 50,
                Stock = 5
            }
        );

        base.OnModelCreating(modelBuilder);
    }
}