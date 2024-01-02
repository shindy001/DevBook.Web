using DevBook.Web.ApiService;
using DevBook.Web.ApiService.Identity;
using DevBook.Web.ApiService.Infrastructure;
using DevBook.Web.ApiService.Middleware;
using DevBook.Web.Shared.Extensions;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddInfrastructure();
builder.Services.AddSwaggerGen(SwaggerOptions.WithBearerAuthorization());
builder.Services.AddEndpointsApiExplorer()
	.ConfigureHttpJsonOptions(opt
		=> opt.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);

builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();
builder.Services.AddIdentityCore<DevBookUser>()
	.AddEntityFrameworkStores<DevBookDbContext>()
	.AddApiEndpoints();

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

app.MapGroup("/identity")
	.MapIdentityApi<DevBookUser>()
	.WithTags($"Identity");

app.MapFeatureModulesEndpoints();

app.Run();
