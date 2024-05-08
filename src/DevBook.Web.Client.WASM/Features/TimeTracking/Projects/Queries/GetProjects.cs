namespace DevBook.Web.Client.WASM.Features.TimeTracking.Projects.Queries;

internal static class GetProjects
{
	internal sealed record Query : IRequest<OneOf<IEnumerable<Project>, DevBookError>>;

	private sealed class Handler(IDevBookWebApiGraphQLClient client) : IRequestHandler<Query, OneOf<IEnumerable<Project>, DevBookError>>
	{
		public async Task<OneOf<IEnumerable<Project>, DevBookError>> Handle(Query request, CancellationToken cancellationToken)
		{
			var result = await client.GetProjects.ExecuteAsync(cancellationToken);

			return result.Unwrap(() => result.Data?.Projects.Select(x => new Project(x.Id, x.Name, x.Details, x.HourlyRate, x.Currency, x.HexColor)) ?? []);
		}
	}
}

