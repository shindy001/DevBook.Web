namespace DevBook.Web.Client.WASM.Features.TimeTracking.Tasks.Commands;

internal static class StopTask
{
	internal sealed record Command(
		Guid Id,
		TimeSpan End)
		: IRequest<OneOf<Success, DevBookError>>;

	private sealed class Handler(IDevBookWebApiGraphQLClient client) : IRequestHandler<Command, OneOf<Success, DevBookError>>
	{
		public async Task<OneOf<Success, DevBookError>> Handle(Command request, CancellationToken cancellationToken)
		{
			var result = await client.StopTask.ExecuteAsync(
					new()
					{
						Id = request.Id,
						End = request.End
					},
				cancellationToken);

			return result.Unwrap();
		}
	}
}
