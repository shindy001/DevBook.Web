namespace DevBook.Web.Client.WASM.Features.TimeTracking.Tasks.Queries;

internal static class GetTask
{
	internal sealed record Query(Guid Id) : IRequest<OneOf<WorkTask, DevBookError>>;

	private sealed class Handler(IDevBookWebApiGraphQLClient client) : IRequestHandler<Query, OneOf<WorkTask, DevBookError>>
	{
		public async Task<OneOf<WorkTask, DevBookError>> Handle(Query request, CancellationToken cancellationToken)
		{
			var result = await client.GetWorkTask.ExecuteAsync(new() { Id = request.Id }, cancellationToken);
			var workTask = result.Data?.WorkTask as GetWorkTask_WorkTask_WorkTaskDto;

			if (result.IsErrorResult() || workTask is null)
			{
				return result.CreateError();
			}

			return result.Unwrap(() =>
				new WorkTask(
					workTask.Id,
					workTask.Project is null ? null : new Project(workTask.Project.Id, workTask.Project.Name, string.Empty),
					workTask.Description,
					workTask.Details,
					workTask.Date,
					workTask.Start,
					workTask.End));
		}
	}
}
