using DevBook.Web.ApiService.Features.TimeTracking.Projects;
using DevBook.Web.Shared.Contracts;

namespace DevBook.Web.ApiService.Features.TimeTracking;

internal sealed class TimeTrackingModule : IFeatureModule
{
	public IServiceCollection RegisterModule(IServiceCollection builder)
	{
		return builder;
	}

	public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpointsBuilder)
	{
		ProjectEndpoints.Map(endpointsBuilder);

		return endpointsBuilder;
	}
}
