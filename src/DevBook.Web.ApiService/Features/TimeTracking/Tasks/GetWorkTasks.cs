using DevBook.Web.ApiService.Infrastructure;
using DevBook.Web.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DevBook.Web.ApiService.Features.TimeTracking.Tasks;

internal sealed record GetWorkTasksQuery : IQuery<IEnumerable<WorkTask>>;

internal sealed class GetWorkTasksQueryHandler(DevBookDbContext dbContext) : IQueryHandler<GetWorkTasksQuery, IEnumerable<WorkTask>>
{
	public async Task<IEnumerable<WorkTask>> Handle(GetWorkTasksQuery request, CancellationToken cancellationToken)
	{
		// TODO - implement paging
		return await dbContext.Tasks.ToListAsync(cancellationToken);
	}
}
