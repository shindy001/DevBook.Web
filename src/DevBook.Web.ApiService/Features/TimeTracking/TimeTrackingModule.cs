using DevBook.Web.Shared.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DevBook.Web.ApiService.Features.TimeTracking;

internal sealed class TimeTrackingModule : IFeatureModule
{
	private const string GetProjectByIdAction = "GetProjectById";

	public IServiceCollection RegisterModule(IServiceCollection builder)
	{
		return builder;
	}

	public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet("/projects", GetProjects)
			.Produces<IList<Project>>();

		endpoints.MapPost("/projects", CreateProject)
			.Produces(StatusCodes.Status201Created);

		endpoints.MapGet("/projects/{id:guid}", GetProjectById)
			.WithName(GetProjectByIdAction)
			.Produces<Project>()
			.Produces(StatusCodes.Status404NotFound);

		endpoints.MapPut("/projects/{id:guid}", UpdateProject)
			.Produces(StatusCodes.Status204NoContent)
			.Produces(StatusCodes.Status404NotFound);

		endpoints.MapPatch("/projects/{id:guid}", PatchProject)
			.Produces(StatusCodes.Status204NoContent)
			.Produces(StatusCodes.Status404NotFound);

		endpoints.MapDelete("/projects/{id:guid}", DeleteProject)
			.Produces(StatusCodes.Status204NoContent);

		return endpoints;
	}

	private static async Task<IResult> GetProjects(IExecutor executor, CancellationToken cancellationToken)
	{
		var result = await executor.ExecuteQuery(new GetProjectsQuery(), cancellationToken);
		return TypedResults.Ok(result);
	}

	private static async Task<IResult> CreateProject(CreateProjectCommand command, IExecutor executor, CancellationToken cancellationToken)
	{
		var result = await executor.ExecuteCommand(command, cancellationToken);
		return TypedResults.CreatedAtRoute(GetProjectByIdAction, new { id = result });
	}

	private static async Task<IResult> GetProjectById(Guid id, IExecutor executor, CancellationToken cancellationToken)
	{
		var result = await executor.ExecuteQuery(new GetProjectQuery(id), cancellationToken);
		return result.Match<IResult>(
			project => TypedResults.Ok(project),
			notFound => TypedResults.NotFound(id));
	}

	private static async Task<IResult> UpdateProject([FromRoute] Guid id, UpdateProjectCommandDto command, IExecutor executor, CancellationToken cancellationToken)
	{
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
			success => TypedResults.NoContent(),
			notFound => TypedResults.NotFound(id));
	}

	private static async Task<IResult> PatchProject([FromRoute] Guid id, PatchProjectCommandDto command, IExecutor executor, CancellationToken cancellationToken)
	{
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
			success => TypedResults.NoContent(),
			notFound => TypedResults.NotFound(id));
	}

	private static async Task<IResult> DeleteProject([FromRoute] Guid id, IExecutor executor, CancellationToken cancellationToken)
	{
		await executor.ExecuteCommand(new DeleteProjectCommand(id), cancellationToken);
		return TypedResults.NoContent();
	}
}
