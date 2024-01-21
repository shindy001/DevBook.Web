using DevBook.Web.ApiService.Features.TimeTracking.Shared;
using DevBook.Web.ApiService.Infrastructure;
using DevBook.Web.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DevBook.Web.ApiService.Features.TimeTracking.Tasks;

internal sealed record GetWorkTasksQuery : IQuery<IEnumerable<WorkTaskDto>>;

internal sealed class GetWorkTasksQueryHandler(DevBookDbContext dbContext) : IQueryHandler<GetWorkTasksQuery, IEnumerable<WorkTaskDto>>
{
	public async Task<IEnumerable<WorkTaskDto>> Handle(GetWorkTasksQuery request, CancellationToken cancellationToken)
	{
		// TODO - implement paging
		var tasks = await dbContext.Tasks.ToListAsync(cancellationToken);
		var projectsIds = tasks.Select(x => x.ProjectId);
		var projects = projectsIds.Any()
			? (await dbContext.Projects.Where(proj => projectsIds.Contains(proj.Id)).ToDictionaryAsync(x => x.Id, x => x, cancellationToken))
			: [];

		return tasks.Select(
			task => new WorkTaskDto(
				Id: task.Id,
				ProjectIdentity: task.ProjectId is not null && projects.TryGetValue(task.ProjectId.Value, out var project) ? project.Identity() : null,
				Description: task.Description,
				Details: task.Details,
				Date: task.Date,
				Start: task.Start,
				End: task.End));
	}
}
