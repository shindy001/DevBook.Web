using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DevBook.Web.ServiceDefaults;

public static class FeatureModuleExtensions
{
	static readonly List<IFeatureModule> registeredModules = [];

	/// <summary>
	/// Registeres modules that implement <see cref="IFeatureModule"/> contract
	/// </summary>
	/// <param name="services"></param>
	/// <returns></returns>
	public static IServiceCollection RegisterFeatureModules(this IServiceCollection services, params Assembly[] commandAndQueriesAssemblies)
	{
		foreach (var assembly in commandAndQueriesAssemblies)
		{
			RegisterModules(services, assembly);
		}

		return services;
	}

	/// <summary>
	/// Maps <see cref="IFeatureModule"/> enpoints
	/// </summary>
	/// <param name="app"></param>
	/// <returns></returns>
	public static WebApplication MapFeatureModulesEndpoints(this WebApplication app)
	{
		foreach (var module in registeredModules)
		{
			module.MapEndpoints(app);
		}

		return app;
	}

	private static void RegisterModules(IServiceCollection services, Assembly assembly)
	{
		var modules = DiscoverModules(assembly);
		foreach (var module in modules)
		{
			module.RegisterModule(services);
			registeredModules.Add(module);
		}
	}

	private static IEnumerable<IFeatureModule> DiscoverModules(Assembly assembly)
	{
		return assembly
			.GetTypes()
			.Where(t => t.IsClass && t.IsAssignableTo(typeof(IFeatureModule)))
			.Select(Activator.CreateInstance)
			.Cast<IFeatureModule>();
	}
}
