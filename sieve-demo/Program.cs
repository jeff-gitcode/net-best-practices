using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using sieve_demo.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Add Sieve services
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("SieveDemo"));
builder.Services.AddScoped<SieveProcessor>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

using (var scope = app.Services.CreateAsyncScope())
{
    await scope.ServiceProvider.GetService<ApplicationDbContext>()!.Database.EnsureCreatedAsync();
}

app.Run();
