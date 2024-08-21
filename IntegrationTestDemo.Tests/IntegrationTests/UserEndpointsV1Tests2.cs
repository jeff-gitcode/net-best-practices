using AutoFixture.Xunit2;
using IntegrationTestDemo.User;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace IntegrationTestDemo.Tests.IntegrationTests;

[Collection("Sequential")]
public class UserEndpointsV1Tests2 : IClassFixture<IntegrationTestWebApplicationFactory2<Program>>
{
    private readonly IntegrationTestWebApplicationFactory2<Program> _factory;
    private readonly HttpClient _httpClient;

    public UserEndpointsV1Tests2(IntegrationTestWebApplicationFactory2<Program> factory)
    {
        _factory = factory;
        _httpClient = factory!.CreateClient();
    }

    [Theory, AutoData]
    public async Task GetUsers(List<UserEntity> expected)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            // etc.
        };

        var url = "https://jsonplaceholder.typicode.com/users";

        _factory.StubHttpRequest<List<UserEntity>>(url,
            HttpStatusCode.OK,
           expected);

        var response = await _httpClient.GetAsync("/useritems/v1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        // Assert.Equal(url, response.Headers.Location.OriginalString);

        var content = await response.Content.ReadAsStringAsync();

        var jsonContent = await response.Content.ReadFromJsonAsync<List<UserEntity>>();

        var actual = JsonSerializer.Deserialize<List<UserEntity>>(content, options);

        Assert.Equivalent(expected, actual);
    }


}