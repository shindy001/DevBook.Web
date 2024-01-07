var builder = DistributedApplication.CreateBuilder(args);

var apiservice = builder.AddProject<Projects.DevBook_Web_ApiService>("apiservice");

builder.AddProject<Projects.DevBook_Web_Client_WASM>("wasm_client")
	.WithReference(apiservice);

builder.Build().Run();
