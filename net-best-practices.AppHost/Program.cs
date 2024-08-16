var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.serilog_demo>("serilog-demo");
builder.AddProject<Projects.IntegrationTestDemo>("IntegrationTestDemo");

builder.Build().Run();
