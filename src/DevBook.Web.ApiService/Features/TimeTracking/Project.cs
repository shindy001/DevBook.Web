using DevBook.Web.Shared;

namespace DevBook.Web.ApiService.Features.TimeTracking;

public sealed record Project(string Name, string? Details, int? HourlyRate, string? Currency, string? HexColor)
	: Entity(Guid.NewGuid());
