using DevBook.Web.ApiService.Infrastructure;
using DevBook.Web.ApiService.Middleware;
using DevBook.Web.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer()
	.ConfigureHttpJsonOptions(opt 
		=> opt.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);

builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure();
builder.Services.RegisterFeatureModules([typeof(Program).Assembly]);

var app = builder.Build();

// Create DB if not exist or migrate if not up to date
app.InitializeDb(applyMigrations: true);

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();

	app.UseSwagger();
	app.UseSwaggerUI(opt =>
	{
		opt.InjectStylesheet("/swagger-ui/SwaggerDark.css");
	});
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

// Health check + Alive
app.MapDefaultEndpoints();

app.MapFeatureModulesEndpoints();

app.Run();
