using IntegrationTestDemo.Todo;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTestDemo.Tests.UnitTests;

public class ToDoTests
{
    [Fact]
    public async Task GetTodoReturnsNotFoundIfNotExists()
    {
        await using var context = new MockDb().CreateDbContext();

        var result = await TodoEndpointsV1.GetTodo(1, context);

        //Assert
        Assert.IsType<Results<Ok<TodoEntity>, NotFound>>(result);

        var notFoundResult = (NotFound)result.Result;

        Assert.NotNull(notFoundResult);
    }

    [Fact]
    public async Task GetAllReturnsTodosFromDatabase()
    {
        // Arrange
        await using var context = new MockDb().CreateDbContext();

        context.Todos.Add(new TodoEntity
        {
            Id = 1,
            Name = "Test1",
            IsComplete = true,
        });

        context.Todos.Add(new TodoEntity
        {
            Id = 2,
            Name = "Test2",
            IsComplete = false,
        });

        await context.SaveChangesAsync();

        // Act
        var result = await TodoEndpointsV1.GetAllTodos(context);

        //Assert
        Assert.IsType<Ok<List<TodoEntity>>>(result);

        Assert.NotNull(result.Value);
        Assert.NotEmpty(result.Value);
        Assert.Collection(result.Value, todo1 =>
        {
            Assert.Equal(1, todo1.Id);
            Assert.Equal("Test1", todo1.Name);
            Assert.True(todo1.IsComplete);
        }, todo2 =>
        {
            Assert.Equal(2, todo2.Id);
            Assert.Equal("Test2", todo2.Name);
            Assert.False(todo2.IsComplete);
        });
    }

    [Fact]
    public async Task GetTodoReturnsTodoFromDatabase()
    {
        // Arrange
        await using var context = new MockDb().CreateDbContext();

        context.Todos.Add(new TodoEntity
        {
            Id = 1,
            Name = "Test1",
            IsComplete = true,
        });

        await context.SaveChangesAsync();

        // Act
        var result = await TodoEndpointsV1.GetTodo(1, context);

        //Assert
        Assert.IsType<Results<Ok<TodoEntity>, NotFound>>(result);

        var okResult = (Ok<TodoEntity>)result.Result;

        Assert.NotNull(okResult.Value);
        Assert.Equal(1, okResult.Value.Id);
    }

    [Fact]
    public async Task CreateTodoCreatesTodoInDatabase()
    {
        //Arrange
        await using var context = new MockDb().CreateDbContext();

        var newTodo = new TodoEntity
        {
            Name = "Test1",
            IsComplete = true,
        };

        //Act
        var result = await TodoEndpointsV1.CreateTodo(newTodo, context);

        //Assert
        Assert.IsType<Created<TodoEntity>>(result);

        Assert.NotNull(result);
        Assert.NotNull(result.Location);

        Assert.NotEmpty(context.Todos);
        Assert.Collection(context.Todos, todo =>
        {
            Assert.Equal("Test1", newTodo.Name);
            Assert.True(newTodo.IsComplete);
        });
    }

    [Fact]
    public async Task UpdateTodoUpdatesTodoInDatabase()
    {
        //Arrange
        await using var context = new MockDb().CreateDbContext();

        context.Todos.Add(new TodoEntity
        {
            Id = 1,
            Name = "Test1",
            IsComplete = true,
        });

        await context.SaveChangesAsync();

        var updatedTodo = new TodoEntity
        {
            Id = 1,
            Name = "Update Test1",
            IsComplete = true,
        };

        //Act
        var result = await TodoEndpointsV1.UpdateTodo(1, updatedTodo, context);

        //Assert
        Assert.IsType<Results<Created<TodoEntity>, NotFound>>(result);

        var createdResult = (Created<TodoEntity>)result.Result;

        Assert.NotNull(createdResult);
        Assert.NotNull(createdResult.Location);

        var todoInDb = await context.Todos.FindAsync(1);

        Assert.NotNull(todoInDb);
        Assert.Equal("Update Test1", todoInDb!.Name);
        Assert.True(todoInDb.IsComplete);
    }

    [Fact]
    public async Task DeleteTodoDeletesTodoInDatabase()
    {
        //Arrange
        await using var context = new MockDb().CreateDbContext();

        var existingTodo = new TodoEntity
        {
            Id = 1,
            Name = "Test1",
            IsComplete = true,
        };

        context.Todos.Add(existingTodo);

        await context.SaveChangesAsync();

        //Act
        var result = await TodoEndpointsV1.DeleteTodo(existingTodo.Id, context);

        //Assert
        Assert.IsType<Results<NoContent, NotFound>>(result);

        var noContentResult = (NoContent)result.Result;

        Assert.NotNull(noContentResult);
        Assert.Empty(context.Todos);
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