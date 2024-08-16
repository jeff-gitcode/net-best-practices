using IntegrationTestDemo.Todo;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTestDemo.Tests;

public class ToDoTests
{
    [Fact]
    public async Task Test1()
    {
        await using var context = new MockDb().CreateDbContext();
    }
}

public class MockDb : IDbContextFactory<TodoDbContext>
{
    public TodoDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase($"InMemoryTestDb-{DateTime.Now.ToFileTimeUtc()}")
            .Options;

        return new TodoDbContext(options);
    }
}