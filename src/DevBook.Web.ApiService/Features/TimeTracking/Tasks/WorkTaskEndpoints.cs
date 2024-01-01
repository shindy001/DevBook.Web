using DevBook.Web.ApiService.Features.TimeTracking.Tasks;
using DevBook.Web.Shared.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DevBook.Web.ApiService.Features.TimeTracking.Projects;

internal static class WorkTaskEndpoints
{
	private const string GetWorkTaskByIdAction = "GetWorkTaskById";

	public static RouteGroupBuilder MapWorkTaskEndpoints(this RouteGroupBuilder groupBuilder)
	{
		groupBuilder.MapGet("/", GetWorkTasks)
			.Produces<IList<WorkTask>>();

		groupBuilder.MapPost("/", CreateWorkTask)
			.Produces(StatusCodes.Status201Created);

		groupBuilder.MapGet("/{id:guid}", GetWorkTaskById)
			.WithName(GetWorkTaskByIdAction)
			.Produces<WorkTask>()
			.Produces(StatusCodes.Status404NotFound);

		groupBuilder.MapDelete("/{id:guid}", DeleteWorkTask)
			.Produces(StatusCodes.Status204NoContent);

		return groupBuilder;
	}

	private static async Task<IResult> GetWorkTasks(IExecutor executor, CancellationToken cancellationToken)
	{
		var result = await executor.ExecuteQuery(new GetWorkTasksQuery(), cancellationToken);
		return TypedResults.Ok(result);
	}

	private static async Task<IResult> CreateWorkTask(CreateWorkTaskCommand command, IExecutor executor, CancellationToken cancellationToken)
	{
		var result = await executor.ExecuteCommand(command, cancellationToken);
		return TypedResults.CreatedAtRoute(GetWorkTaskByIdAction, new { id = result });
	}

	private static async Task<IResult> GetWorkTaskById(Guid id, IExecutor executor, CancellationToken cancellationToken)
	{
		var result = await executor.ExecuteQuery(new GetWorkTaskQuery(id), cancellationToken);
		return result.Match<IResult>(
			project => TypedResults.Ok(project),
			notFound => TypedResults.NotFound(id));
	}

	private static async Task<IResult> DeleteWorkTask([FromRoute] Guid id, IExecutor executor, CancellationToken cancellationToken)
	{
		await executor.ExecuteCommand(new DeleteWorkTaskCommand(id), cancellationToken);
		return TypedResults.NoContent();
	}
}
