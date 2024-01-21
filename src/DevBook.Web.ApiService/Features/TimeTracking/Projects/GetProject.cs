using DevBook.Web.ApiService.Features.TimeTracking.Shared;
using DevBook.Web.ApiService.Infrastructure;
using DevBook.Web.Shared.Contracts;
using OneOf;
using OneOf.Types;

namespace DevBook.Web.ApiService.Features.TimeTracking.Projects;

internal record GetProjectQuery(Guid Id) : IQuery<OneOf<Project, NotFound>>;

internal class GetProjectQueryHandler(DevBookDbContext dbContext) : IQueryHandler<GetProjectQuery, OneOf<Project, NotFound>>
{
	public async Task<OneOf<Project, NotFound>> Handle(GetProjectQuery request, CancellationToken cancellationToken)
	{
		var project = await dbContext.Projects.FindAsync([request.Id], cancellationToken);

		return project is null
			? new NotFound()
			: project;
	}
}
