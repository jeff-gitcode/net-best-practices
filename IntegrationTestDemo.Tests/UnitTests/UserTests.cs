using IntegrationTestDemo.User;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTestDemo.Tests.UnitTests;

public class UserTests
{
    [Fact]
    public async Task GetAllReturnsUsersFromDatabase()
    {
        // Arrange
        var httpClient = new HttpClient();
        await using var context = new MockUserDb().CreateDbContext();

        context.Users.Add(new UserEntity
        {
            Id = 1,
            Name = "Test1",
            Email = "email",
        });

        context.Users.Add(new UserEntity
        {
            Id = 2,
            Name = "Test2",
            Email = "email",
        });

        await context.SaveChangesAsync();

        // Act
        var result = await UserEndpointsV1.GetAll(context, httpClient);

        //Assert
        Assert.IsType<Ok<List<UserEntity>>>(result);

        Assert.NotNull(result.Value);
        Assert.NotEmpty(result.Value);
        Assert.Collection(result.Value, user1 =>
        {
            Assert.Equal(1, user1.Id);
            Assert.Equal("Test1", user1.Name);
            Assert.Equal("email", user1.Email);
        }, user2 =>
        {
            Assert.Equal(2, user2.Id);
            Assert.Equal("Test2", user2.Name);
            Assert.Equal("email", user2.Email);
        });
    }

    [Fact]
    public async Task GetUserReturnsUserFromDatabase()
    {
        // Arrange
        await using var context = new MockUserDb().CreateDbContext();

        context.Users.Add(new UserEntity
        {
            Id = 1,
            Name = "Test1",
            Email = "email",
        });

        await context.SaveChangesAsync();

        // Act
        var result = await UserEndpointsV1.GetById(1, context);

        //Assert
        Assert.IsType<Results<Ok<UserEntity>, NotFound>>(result);

        var okResult = (Ok<UserEntity>)result.Result;

        Assert.NotNull(okResult.Value);
        Assert.Equal(1, okResult.Value.Id);
    }

    [Fact]
    public async Task CreateUserCreatesUserInDatabase()
    {
        //Arrange
        await using var context = new MockUserDb().CreateDbContext();

        var newUser = new UserEntity
        {
            Name = "Test1",
            Email = "email",
        };

        //Act
        var result = await UserEndpointsV1.Create(newUser, context);

        //Assert
        Assert.IsType<Created<UserEntity>>(result);

        Assert.NotNull(result);
        Assert.NotNull(result.Location);

        Assert.NotEmpty(context.Users);
        Assert.Collection(context.Users, user =>
        {
            Assert.Equal("Test1", newUser.Name);
            Assert.Equal("email", newUser.Email);
        });
    }

    [Fact]
    public async Task UpdateUserUpdatesUserInDatabase()
    {
        //Arrange
        await using var context = new MockUserDb().CreateDbContext();

        context.Users.Add(new UserEntity
        {
            Id = 1,
            Name = "Test1",
            Email = "email",
        });

        await context.SaveChangesAsync();

        var updatedUser = new UserEntity
        {
            Id = 1,
            Name = "Update Test1",
            Email = "email",
        };

        //Act
        var result = await UserEndpointsV1.Update(1, updatedUser, context);

        //Assert
        Assert.IsType<Results<Created<UserEntity>, NotFound>>(result);

        var createdResult = (Created<UserEntity>)result.Result;

        Assert.NotNull(createdResult);
        Assert.NotNull(createdResult.Location);

        var userInDb = await context.Users.FindAsync(1);

        Assert.NotNull(userInDb);
        Assert.Equal("Update Test1", userInDb!.Name);
        Assert.Equal("email", userInDb.Email);
    }

    [Fact]
    public async Task DeleteUserDeletesUserInDatabase()
    {
        //Arrange
        await using var context = new MockUserDb().CreateDbContext();

        var existingUser = new UserEntity
        {
            Id = 1,
            Name = "Test1",
            Email = "email",
        };

        context.Users.Add(existingUser);

        await context.SaveChangesAsync();

        //Act
        var result = await UserEndpointsV1.Delete(existingUser.Id, context);

        //Assert
        Assert.IsType<Results<NoContent, NotFound>>(result);

        var noContentResult = (NoContent)result.Result;

        Assert.NotNull(noContentResult);
        Assert.Empty(context.Users);
    }

}

public class MockUserDb : IDbContextFactory<UserDbContext>
{
    public UserDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase($"InMemoryTestDb-{DateTime.Now.ToFileTimeUtc()}")
            .Options;

        return new UserDbContext(options);
    }
}