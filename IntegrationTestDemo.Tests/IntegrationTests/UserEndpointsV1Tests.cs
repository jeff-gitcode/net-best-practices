using AutoFixture.Xunit2;
using IntegrationTestDemo.User;
using Microsoft.EntityFrameworkCore;
using RichardSzalay.MockHttp;
using System.Net.Http.Json;
using System.Text.Json;

namespace IntegrationTestDemo.Tests.IntegrationTests;

[Collection("Sequential")]
public class UserEndpointsV1Tests : IClassFixture<IntegrationTestWebApplicationFactory<Program>>
{
    private readonly IntegrationTestWebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public UserEndpointsV1Tests(IntegrationTestWebApplicationFactory<Program> factory)
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

        _factory.MockHttpHandler.When(url).RespondJson<List<UserEntity>>(expected);

        var response = await _httpClient.GetAsync("/useritems/v1");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var jsonContent = await response.Content.ReadFromJsonAsync<List<UserEntity>>();

        var actual = JsonSerializer.Deserialize<List<UserEntity>>(content, options);

        Assert.Equivalent(expected, actual);
    }

    [Theory, AutoData]
    public async Task AddUsers(UserEntity expected)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            // etc.
        };

        var url = "https://jsonplaceholder.typicode.com/users";

        _factory.MockHttpHandler.When(url).RespondJson<UserEntity>(expected);

        var response = await _httpClient.PostAsJsonAsync("/useritems/v1", expected);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var jsonContent = await response.Content.ReadFromJsonAsync<UserEntity>();

        var actual = JsonSerializer.Deserialize<UserEntity>(content, options);

        Assert.Equivalent(expected, actual);

        Assert.Equal(2, await _factory.GetContext().Users.CountAsync());
    }

}