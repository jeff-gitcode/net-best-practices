var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.serilog_demo>("serilog-demo");

builder.Build().Run();
