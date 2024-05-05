namespace DevBook.Web.Client.WASM.Features.TimeTracking;

public sealed record Project(Guid Id, string Name, int? HourlyRate = null, string? Currency = null, string? HexColor = null)
{
	public static Project FromDto(ProjectDto dto) => new(dto.Id, dto.Name, dto.HourlyRate, dto.Currency, dto.HexColor);
}
