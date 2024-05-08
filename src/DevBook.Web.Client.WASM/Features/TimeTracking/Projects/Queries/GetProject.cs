namespace DevBook.Web.Client.WASM.Features.TimeTracking.Projects.Queries;

internal static class GetProject
{
	internal sealed record Query(Guid Id) : IRequest<OneOf<Project, DevBookError>>;

	private sealed class Handler(IDevBookWebApiGraphQLClient client) : IRequestHandler<Query, OneOf<Project, DevBookError>>
	{
		public async Task<OneOf<Project, DevBookError>> Handle(Query request, CancellationToken cancellationToken)
		{
			var result = await client.GetProject.ExecuteAsync(new() { Id = request.Id }, cancellationToken);
			return result.Data?.Project switch
			{
				IGetProject_Project_ProjectDto proj => new Project(proj.Id, proj.Name, proj.Details, proj.HourlyRate, proj.Currency, proj.HexColor),
				IGetProject_Project_NotFoundError => new DevBookError(Description: $"Project not found"),
				_ => throw new DevBookException()
			};
		}
	}
}

