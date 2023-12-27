using DevBook.Web.Shared.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DevBook.Web.ApiService.Features.TimeTracking;

internal sealed class TimeTrackingModule : IFeatureModule
{
	public IServiceCollection RegisterModule(IServiceCollection builder)
	{
		return builder;
	}

	public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet(
			"/projects",
			async ([FromServices] IExecutor executor, CancellationToken cancellationToken)
				=> await executor.ExecuteQuery(new GetProjectsQuery(), cancellationToken));

		endpoints.MapPost(
			"/projects",
			async (CreateProjectCommand command, [FromServices] IExecutor executor, CancellationToken cancellationToken)
				=> await executor.ExecuteCommand(command, cancellationToken));

		endpoints.MapGet(
			"/projects/{id:guid}",
			async (Guid id, [FromServices] IExecutor executor, CancellationToken cancellationToken)
				=>
			{
				var result = await executor.ExecuteQuery(new GetProjectQuery(id), cancellationToken);
				return result.Match<IResult>(
					project => TypedResults.Ok(project),
					notFound => TypedResults.NotFound(id));
			});

		endpoints.MapPut(
			"/projects/{id:guid}",
			async ([FromRoute] Guid id, UpdateProjectCommandDto command, [FromServices] IExecutor executor, CancellationToken cancellationToken)
				=> {
					var result = await executor.ExecuteCommand(
						new UpdateProjectCommand(
							id,
							command.Name,
							command.Details,
							command.HourlyRate,
							command.Currency,
							command.HexColor),
						cancellationToken); 
					
					return result.Match<IResult>(
						success => TypedResults.Ok(),
						notFound => TypedResults.NotFound(id));
				});

		endpoints.MapPatch(
			"/projects/{id:guid}",
			async ([FromRoute] Guid id, PatchProjectCommandDto command, [FromServices] IExecutor executor, CancellationToken cancellationToken)
				=> {
                    var result = await executor.ExecuteCommand(
						new PatchProjectCommand(
							id,
							command.Name,
							command.Details,
							command.HourlyRate,
							command.Currency,
							command.HexColor),
						cancellationToken);

					return result.Match<IResult>(
						success => TypedResults.Ok(),
						notFound => TypedResults.NotFound(id));
                });

		return endpoints;
	}
}
