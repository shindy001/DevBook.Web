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
	/// <param name="services"></param>
	/// <returns></returns>
	IServiceCollection RegisterModule(IServiceCollection services);

	/// <summary>
	/// Map endpoints required by the module
	/// </summary>
	/// <param name="endpointsBuilder"></param>
	/// <returns></returns>
	IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpointsBuilder);
}
