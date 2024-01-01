using DevBook.Web.Shared;

namespace DevBook.Web.ApiService.Features.TimeTracking.Tasks;

public sealed record WorkTask(
	Guid? ProjectId,
	string? Description,
	string? Details,
	DateOnly? Date,
	TimeOnly Start,
	TimeOnly? End)
	: Entity(Guid.NewGuid());
