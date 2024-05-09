namespace DevBook.Web.Client.WASM.Features.TimeTracking.Tasks.Queries;

internal static class ListTasks
{
	internal sealed record Model
	{
		public WorkTask? RunningTask { get; set; } = null;
		public IEnumerable<TasksInDay> TasksInDays { get; set; } = [];

		public sealed record TasksInDay
		{
			public DateOnly Day { get; init; }
			public WorkTask[] Tasks { get; init; }

			public TasksInDay(DateOnly day, WorkTask[] tasks)
			{
				Day = day;
				Tasks = tasks;
			}
		}
	}

	internal sealed record Query : IRequest<OneOf<Model, DevBookError>>;

	private sealed class Handler(IDevBookWebApiGraphQLClient client) : IRequestHandler<Query, OneOf<Model, DevBookError>>
	{
		public async Task<OneOf<Model, DevBookError>> Handle(Query request, CancellationToken cancellationToken)
		{
			var result = await client.ListTasks.ExecuteAsync(cancellationToken);

			return result.Unwrap(() =>
			{
				var activeWorkTask = result.Data?.WorkTaskList.ActiveWorkTask;

				return new Model
				{
					RunningTask = activeWorkTask is null ? null : new WorkTask(
								activeWorkTask.Id,
								activeWorkTask.Project is null
									? null
									: new Project(
										activeWorkTask.Project.Id,
										activeWorkTask.Project.Name,
										activeWorkTask.Project.Details,
										activeWorkTask.Project.HourlyRate,
										activeWorkTask.Project.Currency,
										activeWorkTask.Project.HexColor),
								activeWorkTask.Description,
								activeWorkTask.Details,
								activeWorkTask.Date,
								activeWorkTask.Start,
								activeWorkTask.End),
					TasksInDays = result.Data?.WorkTaskList.WorkTasksInDay?.Select(
						x => new Model.TasksInDay(DateOnly.FromDateTime(x.Key),
						x.Value.Select(x =>
							new WorkTask(
								x.Id,
								x.Project is null ? null : new Project(x.Project.Id, x.Project.Name, x.Project.Details, x.Project.HourlyRate, x.Project.Currency, x.Project.HexColor),
								x.Description,
								x.Details,
								x.Date,
								x.Start,
								x.End)
							).ToArray())) ?? []
				};
			});
		}
	}
}
