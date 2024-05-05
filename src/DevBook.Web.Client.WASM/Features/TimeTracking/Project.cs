namespace DevBook.Web.Client.WASM.Features.TimeTracking;

public sealed record Project(Guid Id, string Name, string? Details, int? HourlyRate = null, string? Currency = null, string? HexColor = null)
{
	public static Project FromDto(ProjectDto dto) => new(dto.Id, dto.Name, dto.Details, dto.HourlyRate, dto.Currency, dto.HexColor);
}
