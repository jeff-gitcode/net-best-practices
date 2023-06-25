using System.Text.Json.Serialization;
using Endpoints;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddTransient<IEndpointRouting, WeatherForecastEndpoints>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");

app.MapGet("person", () =>
    Results.Json(
        new Person("Jeff", Friend: new("JsonTest")),
        PersonSerializationContext.Default.Options));

app.UseHttpsRedirection();

var apis = app.Services.GetServices<IEndpointRouting>();
foreach (var api in apis)
{
    if (api is null) throw new InvalidProgramException("Apis not found");

    api.Register(app);
}

app.Run();

