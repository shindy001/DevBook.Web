using DevBook.Web.ApiService.Features.TimeTracking.Shared;

namespace DevBook.Web.ApiService.Features.TimeTracking.Tasks;

public sealed record WorkTaskDto(
	Guid Id,
	Project? Project,
	string? Description,
	string? Details,
	DateOnly? Date,
	TimeOnly Start,
	TimeOnly? End);
