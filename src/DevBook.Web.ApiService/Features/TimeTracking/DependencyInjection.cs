namespace DevBook.Web.ApiService.Features.TimeTracking;

public static class DependencyInjection
{
	public static IServiceCollection RegisterTimeTrackingServices(this IServiceCollection services)
	{
		return services;
	}

	public static IEndpointRouteBuilder MapTimeTrackingEndpoints(this IEndpointRouteBuilder endpoints)
	{
		return endpoints;
	}
}
