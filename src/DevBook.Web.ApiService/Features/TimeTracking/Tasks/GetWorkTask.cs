using DevBook.Web.ApiService.Infrastructure;
using DevBook.Web.Shared.Contracts;
using OneOf;
using OneOf.Types;

namespace DevBook.Web.ApiService.Features.TimeTracking.Tasks;

internal record GetWorkTaskQuery(Guid Id) : IQuery<OneOf<WorkTask, NotFound>>;

internal class GetWorkTaskQueryHandler(DevBookDbContext dbContext) : IQueryHandler<GetWorkTaskQuery, OneOf<WorkTask, NotFound>>
{
	public async Task<OneOf<WorkTask, NotFound>> Handle(GetWorkTaskQuery request, CancellationToken cancellationToken)
	{
		var workTask = await dbContext.Tasks.FindAsync([request.Id], cancellationToken);

		return workTask is null
			? new NotFound()
			: workTask;
	}
}
