using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace DevBook.Web.Shared.Contracts;

public interface IFeatureModule
{
	IServiceCollection RegisterModule(IServiceCollection builder);
	IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
}
