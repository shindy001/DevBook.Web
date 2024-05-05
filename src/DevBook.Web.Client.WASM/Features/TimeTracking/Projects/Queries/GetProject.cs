namespace DevBook.Web.Client.WASM.Features.TimeTracking.Projects.Queries;

internal static class GetProject
{
	internal sealed record Query(Guid Id) : IRequest<OneOf<Project, DevBookError>>;

	private sealed class Handler(IDevBookWebApiGraphQLClient client) : IRequestHandler<Query, OneOf<Project, DevBookError>>
	{
		public async Task<OneOf<Project, DevBookError>> Handle(Query request, CancellationToken cancellationToken)
		{
			var result = await client.GetProject.ExecuteAsync(new() { Id = request.Id }, cancellationToken);
			var project = result.Data?.Project as GetProject_Project_ProjectDto;

			if (result.IsErrorResult() || project is null)
			{
				return result.CreateError();
			}

			return result.Unwrap(() => new Project(project.Id, project.Name, project.Details, project.HourlyRate, project.Currency, project.HexColor));
		}
	}
}

