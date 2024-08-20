//using IntegrationTestDemo.Todo;
//using Microsoft.Extensions.DependencyInjection;
//using System.Net;
//using System.Net.Http.Json;

//namespace IntegrationTestDemo.Tests.IntegrationTests;

//// Unfortunately the official Microsoft documentation uses xUnit’s ‘IClassFixture’ to share the server instance between tests but that works only for tests located in the same test class. For other classes a new server instance will be created which can make the execution time of the tests significantly longer. So instead of class fixtures we should use collection or assembly fixtures. That is as simply as creating a new file called ‘IntegrationTestCollection.cs’ with the following content:
//// One more thing this collection definition does is that it disables parallel execution of the tests. This is good because we can make sure we won’t reset the database when another test is running.
//// [CollectionDefinition("Integration Tests")]
//// public class IntegrationTestCollection : ICollectionFixture<TestServer>
//// {
//// }

//[Collection("Sequential")]
//public class TodoEndpointsV1IntegrationTests : IClassFixture<IntegrationTestWebApplicationFactory<Program>>
//{
//    private readonly IntegrationTestWebApplicationFactory<Program> _factory;
//    private readonly HttpClient _httpClient;

//    public TodoEndpointsV1IntegrationTests(IntegrationTestWebApplicationFactory<Program> factory)
//    {
//        _factory = factory;
//        _httpClient = factory!.CreateClient();
//    }

//    [Fact]
//    public async Task PostTodoWithValidParameters()
//    {
//        using (var scope = _factory.Services.CreateScope())
//        {
//            var db = scope.ServiceProvider.GetService<TodoDbContext>();
//            if (db != null && db.Todos.Any())
//            {
//                db.Todos.RemoveRange(db.Todos);
//                await db.SaveChangesAsync();
//            }
//        }

//        var response = await _httpClient.PostAsJsonAsync("/todos/v1", new TodoEntity
//        {
//            Title = "Test title",
//            Description = "Test description",
//            IsDone = false
//        });

//        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

//        var todos = await _httpClient.GetFromJsonAsync<List<TodoEntity>>("/todos/v1");

//        Assert.NotNull(todos);
//        Assert.Single(todos);

//        Assert.Collection(todos, (todo) =>
//        {
//            Assert.Equal("Test title", todo.Title);
//            Assert.Equal("Test description", todo.Description);
//            Assert.False(todo.IsDone);
//        });
//    }


//}