using Microsoft.EntityFrameworkCore;

namespace IntegrationTestDemo.User
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options) { }

        public DbSet<UserEntity> Users => Set<UserEntity>();
    }
}