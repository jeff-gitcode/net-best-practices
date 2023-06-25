

using System.Text.Json.Serialization;

public record Person(
    string Name,
    bool IsCool = true,
    Person? Friend = null
);

[JsonSourceGenerationOptions(
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(Person))]
public partial class PersonSerializationContext
    : JsonSerializerContext
{ }