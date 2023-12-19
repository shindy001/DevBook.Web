using Microsoft.EntityFrameworkCore;

namespace DevBook.Web.ApiService.Infrastructure;

internal static class DependencyInjection
{
	internal static IServiceCollection AddInfrastructure(this IServiceCollection services)
	{
		var assembly = typeof(Program).Assembly;

		services.AddDbContextPool<DevBookDbContext>(
			opt => opt.UseSqlite(
				GetSqliteConnectionString(),
				opt => opt.MigrationsAssembly(assembly.GetName().Name)));

		return services;
	}

	internal static IApplicationBuilder InitializeDb(this IApplicationBuilder builder, bool applyMigrations = false)
	{
		using var scope = builder.ApplicationServices.CreateScope();
		var db = scope.ServiceProvider.GetRequiredService<DevBookDbContext>();
		db.Database.EnsureCreated();

		if (applyMigrations)
		{
			db.Database.Migrate();
		}

		return builder;
	}

	private static string GetSqliteConnectionString()
	{
		var dbPath = Path.Combine(AppContext.BaseDirectory, "data", $"DevBook.db");

		if (!Directory.Exists(dbPath))
		{
			Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
		}

		return $"Data Source={dbPath}";
	}
}
