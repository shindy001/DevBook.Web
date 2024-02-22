var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddInfrastructure();
builder.Services.AddSwaggerGen(SwaggerOptions.WithDevBookOptions());
builder.Services.AddEndpointsApiExplorer()
	.ConfigureHttpJsonOptions(opt
		=> opt.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);

builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme, opt => opt.BearerTokenExpiration = TimeSpan.FromSeconds(30));
builder.Services.AddAuthorizationBuilder();
builder.Services.AddIdentityCore<DevBookUser>()
	.AddEntityFrameworkStores<DevBookDbContext>()
	.AddApiEndpoints();

// TODO - also setup cors origins for production
builder.Services.AddCors(options =>
{
	options.AddPolicy("DevBookClient", 
		p => p.WithOrigins("http://localhost:5240", "https://localhost:7136")
		.AllowAnyMethod()
		.SetIsOriginAllowed(isAllowed => true)
		.AllowAnyHeader()
		.AllowCredentials());
});

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

app.UseCors("DevBookClient");
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();

// Health check + Alive
app.MapDefaultEndpoints();

app.MapGroup("/identity")
	.MapIdentityApi<DevBookUser>()
	.WithTags($"Identity");

app.MapFeatureModulesEndpoints();

app.Run();
