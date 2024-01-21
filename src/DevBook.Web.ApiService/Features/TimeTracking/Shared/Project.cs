using DevBook.Web.Shared;
using System.ComponentModel.DataAnnotations;

namespace DevBook.Web.ApiService.Features.TimeTracking.Shared;

public sealed record Project()
	: Entity(Guid.NewGuid())
{
	[Required]
	public required string Name { get; init; }
	public string? Details { get; init; }
	public int? HourlyRate { get; init; }
	public string? Currency { get; init; }
	public string? HexColor { get; init; }
}
