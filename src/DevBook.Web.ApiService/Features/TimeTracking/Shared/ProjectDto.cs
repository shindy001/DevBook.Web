using System.ComponentModel.DataAnnotations;

namespace DevBook.Web.ApiService.Features.TimeTracking.Shared;

public sealed record ProjectDto
{
	public Guid Id { get; }

	[Required]
	public string Name { get; }
	public string? Details { get; }
	public int? HourlyRate { get; }
	public string? Currency { get; }
	public string? HexColor { get; }

	public ProjectDto(Guid id, string name, string? details, int? hourlyRate, string? currency, string? hexColor)
	{
		Id = id;
		Name = name;
		Details = details;
		HourlyRate = hourlyRate;
		Currency = currency;
		HexColor = hexColor;
	}
}
