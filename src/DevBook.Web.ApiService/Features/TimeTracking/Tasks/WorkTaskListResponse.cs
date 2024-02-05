namespace DevBook.Web.ApiService.Features.TimeTracking.Tasks;

public sealed record WorkTaskListResponse
{
	public WorkTaskDto? ActiveWorkTask { get; set; }

	public Dictionary<DateOnly, IEnumerable<WorkTaskDto>> WorkTasksInDay { get; set; } = [];
}
