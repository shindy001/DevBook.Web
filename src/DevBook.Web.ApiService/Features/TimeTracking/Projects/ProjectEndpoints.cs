using DevBook.Web.Shared.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DevBook.Web.ApiService.Features.TimeTracking.Projects;

internal static class ProjectEndpoints
{
	private const string GetProjectByIdAction = "GetProjectById";

	public static RouteGroupBuilder MapProjectEndpoints(this RouteGroupBuilder groupBuilder)
	{
		groupBuilder.MapGet("/", GetProjects)
			.Produces<IList<Project>>();

		groupBuilder.MapPost("/", CreateProject)
			.Produces(StatusCodes.Status201Created);

		groupBuilder.MapGet("/{id:guid}", GetProjectById)
			.WithName(GetProjectByIdAction)
			.Produces<Project>()
			.Produces(StatusCodes.Status404NotFound);

		groupBuilder.MapPut("/{id:guid}", UpdateProject)
			.Produces(StatusCodes.Status204NoContent)
			.Produces(StatusCodes.Status404NotFound);

		groupBuilder.MapPatch("/{id:guid}", PatchProject)
			.Produces(StatusCodes.Status204NoContent)
			.Produces(StatusCodes.Status404NotFound);

		groupBuilder.MapDelete("/{id:guid}", DeleteProject)
			.Produces(StatusCodes.Status204NoContent);

		return groupBuilder;
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
