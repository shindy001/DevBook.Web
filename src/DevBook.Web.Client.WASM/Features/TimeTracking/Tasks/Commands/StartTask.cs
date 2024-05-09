namespace DevBook.Web.Client.WASM.Features.TimeTracking.Tasks.Commands;

internal static class StartTask
{
	internal sealed record Command(
		string? Description,
		DateTimeOffset Date,
		TimeSpan Start)
		: IRequest<OneOf<Success, DevBookError>>;

	private sealed class Handler(IDevBookWebApiGraphQLClient client) : IRequestHandler<Command, OneOf<Success, DevBookError>>
	{
		public async Task<OneOf<Success, DevBookError>> Handle(Command request, CancellationToken cancellationToken)
		{
			var result = await client.StartTask.ExecuteAsync(
					new()
					{
						Description = request.Description,
						Date = request.Date,
						Start = request.Start,
					},
				cancellationToken);

			return result.Unwrap();
		}
	}
}
