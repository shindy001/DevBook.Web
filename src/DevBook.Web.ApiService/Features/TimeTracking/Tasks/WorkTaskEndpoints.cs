﻿using DevBook.Web.Shared.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DevBook.Web.ApiService.Features.TimeTracking.Tasks;

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

		groupBuilder.MapPut("/{id:guid}", UpdateWorkTask)
			.Produces(StatusCodes.Status204NoContent)
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
			workTask => TypedResults.Ok(workTask),
			notFound => TypedResults.NotFound(id));
	}

	private static async Task<IResult> UpdateWorkTask([FromRoute] Guid id, UpdateWorkTaskCommandDto command, IExecutor executor, CancellationToken cancellationToken)
	{
		var result = await executor.ExecuteCommand(
			new UpdateWorkTaskCommand(
				Id: id,
				ProjectId: command.ProjectId,
				Description: command.Description,
				Details: command.Details,
				Date: command.Date,
				Start: command.Start,
				End: command.End),
			cancellationToken);

		return result.Match<IResult>(
			success => TypedResults.NoContent(),
			notFound => TypedResults.NotFound(id));
	}

	private static async Task<IResult> DeleteWorkTask([FromRoute] Guid id, IExecutor executor, CancellationToken cancellationToken)
	{
		await executor.ExecuteCommand(new DeleteWorkTaskCommand(id), cancellationToken);
		return TypedResults.NoContent();
	}
}
