namespace DevBook.Web.Client.WASM.Features.TimeTracking.Projects.Commands;

internal sealed record UpdateProjectCommand(
		Guid Id,
		string Name,
		string? Details,
		string? Currency,
		int? HourlyRate,
		string? HexColor)
		: IRequest<OneOf<Success, DevBookError>>;

internal sealed class UpdateProjectCommandHandler(IDevBookWebApiGraphQLClient client) : IRequestHandler<UpdateProjectCommand, OneOf<Success, DevBookError>>
{
	public async Task<OneOf<Success, DevBookError>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
	{
		var result = await client.UpdateProject.ExecuteAsync(
			new()
			{
				Id = request.Id,
				Name = request.Name,
				Details = request.Details,
				Currency = request.Currency,
				HourlyRate = request.HourlyRate,
				HexColor = request.HexColor
			},
			cancellationToken);

		return result.Unwrap();
	}
}
