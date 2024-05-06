namespace DevBook.Web.Client.WASM.Features.TimeTracking.Tasks.Commands;

internal static class UpdateTask
{
	internal sealed record Command(
		Guid Id,
		Guid? ProjectId,
		string? Description,
		string? Details,
		DateTimeOffset Date,
		TimeSpan Start,
		TimeSpan End)
		: IRequest<OneOf<Success, DevBookError>>;

	private sealed class Handler(IDevBookWebApiGraphQLClient client) : IRequestHandler<Command, OneOf<Success, DevBookError>>
	{
		public async Task<OneOf<Success, DevBookError>> Handle(Command request, CancellationToken cancellationToken)
		{
			var result = await client.UpdateTask.ExecuteAsync(
					new()
					{
						Id = request.Id,
						ProjectId = request.ProjectId,
						Description = request.Description,
						Details = request.Details,
						Date = request.Date,
						Start = request.Start,
						End = request.End
					},
				cancellationToken);

			return result.Unwrap();
		}
	}
}
