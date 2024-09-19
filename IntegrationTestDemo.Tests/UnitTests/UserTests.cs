using AutoFixture.Xunit2;
using IntegrationTestDemo.Tests.IntegrationTests;
using IntegrationTestDemo.User;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using RichardSzalay.MockHttp;

namespace IntegrationTestDemo.Tests.UnitTests;

public class UserTests
{
    [Theory, AutoData]
    public async Task GetAllReturnsUsersFromDatabase(List<UserEntity> expected)
    {
        // Arrange
        string url = $"https://jsonplaceholder.typicode.com/users";
        var mockHttpHandler = new MockHttpMessageHandler();
        mockHttpHandler.When(url).RespondJson<List<UserEntity>>(expected);

        var httpClient = new HttpClient(mockHttpHandler);

        await using var context = new MockUserDb().CreateDbContext();

        // Act
        var result = await UserEndpointsV1.GetAll(context, httpClient);

        //Assert
        Assert.IsType<Ok<List<UserEntity>>>(result);

        Assert.NotNull(result.Value);
        Assert.NotEmpty(result.Value);
        Assert.Equivalent(expected, result.Value);
        //Assert.Collection(result.Value, user1 =>
        //{
        //    Assert.Equal(1, user1.Id);
        //    Assert.Equal("Test1", user1.Name);
        //    Assert.Equal("email", user1.Email);
        //}, user2 =>
        //{
        //    Assert.Equal(2, user2.Id);
        //    Assert.Equal("Test2", user2.Name);
        //    Assert.Equal("email", user2.Email);
        //});
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

    [Theory, AutoData]
    public async Task CreateUserCreatesUserInDatabase(List<UserEntity> expected)
    {
        //Arrange
        await using var context = new MockUserDb().CreateDbContext();

        var newUser = new UserEntity
        {
            Name = "Test1",
            Email = "email",
        };

        string url = $"https://jsonplaceholder.typicode.com/users";
        var mockHttpHandler = new MockHttpMessageHandler();
        mockHttpHandler.When(url).RespondJson<List<UserEntity>>(expected);

        var httpClient = new HttpClient(mockHttpHandler);

        //Act
        var result = await UserEndpointsV1.Create(newUser, context, httpClient);

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

    [Theory, AutoData]
    public async Task UpdateUserUpdatesUserInDatabase(UserEntity expected)
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

        string url = $"https://jsonplaceholder.typicode.com/users";
        var mockHttpHandler = new MockHttpMessageHandler();
        mockHttpHandler.When(url).RespondJson<UserEntity>(updatedUser);

        var httpClient = new HttpClient(mockHttpHandler);

        //Act
        var result = await UserEndpointsV1.Update(1, updatedUser, context, httpClient);

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

    [Theory, AutoData]
    public async Task DeleteUserDeletesUserInDatabase(UserEntity userEntity)
    {
        //Arrange
        await using var context = new MockUserDb().CreateDbContext();

        userEntity.Id = 1;
        context.Users.Add(userEntity);

        await context.SaveChangesAsync();

        string url = $"https://jsonplaceholder.typicode.com/users/1";
        var mockHttpHandler = new MockHttpMessageHandler();
        mockHttpHandler.When(url).RespondJson<UserEntity>((UserEntity)null);

        var httpClient = new HttpClient(mockHttpHandler);

        //Act
        var result = await UserEndpointsV1.Delete(1, context, httpClient);

        //Assert
        Assert.IsType<Results<NoContent, NotFound>>(result);

        var noContentResult = (NoContent)result.Result;

        Assert.NotNull(noContentResult);
        // Assert.Empty(context.Users);
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