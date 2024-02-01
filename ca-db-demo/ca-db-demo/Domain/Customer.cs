namespace Domain;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

public class Customer
{
    [System.ComponentModel.DataAnnotations.Key]
    public string Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; }= string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}

public record BaseEntity
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
}

public record CustomerModel : BaseEntity
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public string Token { get; init; } = string.Empty;
}
