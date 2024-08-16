using Microsoft.EntityFrameworkCore;

namespace IntegrationTestDemo.Todo
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options)
            : base(options) { }

        public DbSet<TodoEntity> Todos => Set<TodoEntity>();
    }
}