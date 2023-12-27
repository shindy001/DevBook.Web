using DevBook.Web.Shared.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DevBook.Web.Shared.Extensions;

public static class FeatureModuleExtensions
{
	static readonly List<IFeatureModule> registeredModules = [];

	public static IServiceCollection RegisterFeatureModules(this IServiceCollection services)
	{
		var modules = DiscoverModules();
		foreach (var module in modules)
		{
			module.RegisterModule(services);
			registeredModules.Add(module);
		}

		return services;
	}

	public static WebApplication MapFeatureModulesEndpoints(this WebApplication app)
	{
		foreach (var module in registeredModules)
		{
			module.MapEndpoints(app);
		}

		return app;
	}

	private static IEnumerable<IFeatureModule> DiscoverModules()
	{
		return typeof(IFeatureModule).Assembly
			.GetTypes()
			.Where(t => t.IsClass && t.IsAssignableTo(typeof(IFeatureModule)))
			.Select(Activator.CreateInstance)
			.Cast<IFeatureModule>();
	}
}
