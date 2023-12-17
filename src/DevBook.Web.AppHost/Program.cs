var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.DevBook_Web_ApiService>("devbook.web.apiservice");

builder.Build().Run();
