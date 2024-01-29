﻿using DevBook.Web.ApiService.Features.TimeTracking.Projects;
using DevBook.Web.ApiService.Features.TimeTracking.Tasks;
using DevBook.Web.ServiceDefaults;

namespace DevBook.Web.ApiService.Features.TimeTracking;

internal sealed class TimeTrackingModule : IFeatureModule
{
	public IServiceCollection RegisterModule(IServiceCollection builder)
	{
		return builder;
	}

	public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpointsBuilder)
	{
		endpointsBuilder
			.MapGroup("/projects")
			.MapProjectEndpoints()
			.WithTags($"{nameof(TimeTrackingModule)}_{nameof(ProjectEndpoints)}")
			.RequireAuthorization();

		endpointsBuilder
			.MapGroup("/tasks")
			.MapWorkTaskEndpoints()
			.WithTags($"{nameof(TimeTrackingModule)}_{nameof(WorkTaskEndpoints)}")
			.RequireAuthorization();

		return endpointsBuilder;
	}
}
