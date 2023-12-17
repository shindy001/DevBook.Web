var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.DevBook_Web_ApiService>("devbook.web.apiservice");

builder.AddProject<Projects.DevBook_Web_Client>("devbook.web.client");

builder.Build().Run();
