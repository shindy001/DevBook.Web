namespace DevBook.Web.Client.WASM.Features.TimeTracking;

public sealed record WorkTask(Guid Id, Project? Project, string? Description, string? Details, DateTimeOffset Date, TimeSpan Start, TimeSpan? End)
{
	public static WorkTask FromDto(WorkTaskDto dto) => new(
		dto.Id,
		dto.Project is null ? null : Project.FromDto(dto.Project),
		dto.Description,
		dto.Details,
		dto.Date,
		dto.Start,
		dto.End);
}
