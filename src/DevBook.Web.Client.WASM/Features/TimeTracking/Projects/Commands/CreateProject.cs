namespace DevBook.Web.Client.WASM.Features.TimeTracking.Projects.Commands;

internal sealed record CreateProjectCommand(
	string Name,
	string? Details,
	string? Currency,
	int? HourlyRate,
	string? HexColor)
	: IRequest<OneOf<Success, DevBookError>>;

internal sealed class CreateProjectCommandHandler(IDevBookWebApiGraphQLClient client) : IRequestHandler<CreateProjectCommand, OneOf<Success, DevBookError>>
{
	public async Task<OneOf<Success, DevBookError>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
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
