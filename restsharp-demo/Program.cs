using RestSharp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.ConfigureServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



app.MapGet("/todo", async () =>
{
    using var scope = app.Services.CreateScope();
    var service = scope.ServiceProvider;
    var client = service.GetRequiredService<IRestService>();
    var request = new RestRequest("todos/1");
    var response = await client.Get<ToDo>(request);
    return Results.Ok(response);
});

app.Run();
