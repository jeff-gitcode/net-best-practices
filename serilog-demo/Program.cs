using Microsoft.AspNetCore.HttpLogging;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using serilog_demo;

// Log.Logger = new LoggerConfiguration()
//     .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {CorrelationId} {Level:u3}] {Message:lj}{NewLine}{Exception}")
//     .Enrich.FromLogContext()
//     .Enrich.WithCorrelationIdHeader("Correlation-Id-Header")
//     .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("Correlation-Id-Header");
    logging.ResponseHeaders.Add("Correlation-Id-Header");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
    // logging.Filter = (ctx, log) => ctx.Request.Path != "/health";
});

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MyMicroservice"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation();
    });

//builder.Host.ConfigureLogging(logging =>
//    {
//        logging.ClearProviders();
//        logging.AddSerilog();
//        logging.SetMinimumLevel(LogLevel.Information);
//    })
//    .UseSerilog();

builder.Host.UseSerilog((ctx, lc) => lc
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {CorrelationId} {Level:u3}] {Message:lj}{NewLine}{Exception}")
        .WriteTo.OpenTelemetry(options =>
        {
            options.Endpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
            var headers = builder.Configuration["OTEL_EXPORTER_OTLP_HEADERS"]?.Split(',') ?? [];
            foreach (var header in headers)
            {
                var (key, value) = header.Split('=') switch
                {
                [string k, string v] => (k, v),
                    var v => throw new Exception($"Invalid header format {v}")
                };

                options.Headers.Add(key, value);
            }
            options.ResourceAttributes.Add("service.name", "apiservice");

            //To remove the duplicate issue, we can use the below code to get the key and value from the configuration

            var (otelResourceAttribute, otelResourceAttributeValue) = builder.Configuration["OTEL_RESOURCE_ATTRIBUTES"]?.Split('=') switch
            {
            [string k, string v] => (k, v),
                _ => throw new Exception($"Invalid header format {builder.Configuration["OTEL_RESOURCE_ATTRIBUTES"]}")
            };

            options.ResourceAttributes.Add(otelResourceAttribute, otelResourceAttributeValue);



        })
        .ReadFrom.Configuration(ctx.Configuration)
);

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<CorrelationIdMiddleware>();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (ILogger<Program> _logger) =>
{
    _logger.LogInformation("Got request at {DateTimeOffset.Now}", DateTimeOffset.Now);
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    _logger.LogInformation("Got request at {At} for forecast: {@forecast}",
        DateTimeOffset.Now, forecast);

    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public partial class Program { }