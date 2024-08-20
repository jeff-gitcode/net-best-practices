using IntegrationTestDemo.Todo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTestDemo.Tests.IntegrationTests;

[Collection("Sequential")]
public class TodoEndpointsV1Tests : IClassFixture<IntegrationTestWebApplicationFactory<Program>>
{
    private readonly IntegrationTestWebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public TodoEndpointsV1Tests(IntegrationTestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = factory!.CreateClient();
    }

    [Fact]
    public async Task PostTodoWithValidParameters()
    {
        using var scope = _factory.Services.CreateScope();
        using var db = scope.ServiceProvider.GetService<TodoDbContext>();
        if (db != null && db.Todos.Any())
        {
            db.Todos.RemoveRange(db.Todos);
            await db.SaveChangesAsync();
        }

        var response = await _httpClient.PostAsJsonAsync("/todoitems/v1", new TodoEntity
        {
            Id = 1,
            Name = "Test1",
            IsComplete = true,
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var todoResult = await response.Content.ReadFromJsonAsync<TodoEntity>();

        var todos = await _httpClient.GetFromJsonAsync<List<TodoEntity>>("/todoitems/v1");
        Assert.NotNull(todos);
        Assert.Single(todos);

        Assert.Collection(todos, (todo) =>
        {
            Assert.Equal(1, todo.Id);
            Assert.Equal("Test1", todo.Name);
            Assert.True(todo.IsComplete);
        });
    }


}