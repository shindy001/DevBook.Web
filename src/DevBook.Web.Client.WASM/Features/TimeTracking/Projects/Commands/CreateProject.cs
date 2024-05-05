namespace DevBook.Web.Client.WASM.Features.TimeTracking.Projects.Commands;


public static class CreateProject
{
	internal sealed record Command(
	string Name,
	string? Details,
	string? Currency,
	int? HourlyRate,
	string? HexColor)
	: IRequest<OneOf<Success, DevBookError>>;

	private sealed class Handler(IDevBookWebApiGraphQLClient client) : IRequestHandler<Command, OneOf<Success, DevBookError>>
	{
		public async Task<OneOf<Success, DevBookError>> Handle(Command request, CancellationToken cancellationToken)
		{
			var result = await client.CreateProject.ExecuteAsync(
				new()
				{
					Name = request.Name,
					Details = request.Details,
					Currency = request.Currency,
					HourlyRate = request.HourlyRate,
					HexColor = request.HexColor
				}, cancellationToken);

			return result.Unwrap();
		}
	}
}
