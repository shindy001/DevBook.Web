using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace DevBook.Web.ServiceDefaults;

/// <summary>
/// Marks feature module
/// Modules are automatically discovered and registered by <see cref="FeatureModuleExtensions"/> methods.
/// </summary>
public interface IFeatureModule
{
	/// <summary>
	/// Register dependencies required by the module
	/// </summary>
	/// <param name="builder"></param>
	/// <returns></returns>
	IServiceCollection RegisterModule(IServiceCollection builder);

	/// <summary>
	/// Map endpoints required by the module
	/// </summary>
	/// <param name="endpoints"></param>
	/// <returns></returns>
	IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
}
