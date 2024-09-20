using IntegrationTestDemo.User;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTestDemo.Tests.IntegrationTests
{
    public interface IDataSeedService
    {
        Task SeedDataAsync();
    }

    public class DataSeedService : IDataSeedService
    {
        private readonly UserDbContext _context;

        public DataSeedService(UserDbContext context)
        {
            _context = context;
        }

        public async Task SeedDataAsync()
        {
            if (!await _context.Users.AnyAsync())
            {
                _context.Users.AddRange(
                    new UserEntity
                    {
                        Id = 1,
                        Name = "Test",
                        Email = "test@email.com",
                        Phone = "12345",
                        Website = "localhost"
                    }
                );
                await _context.SaveChangesAsync();
            }
        }
    }
}
