using DevBook.Web.ApiService.Infrastructure;
using DevBook.Web.Shared.Contracts;

namespace DevBook.Web.ApiService.Features.TimeTracking.Tasks;

internal sealed record CreateWorkTaskCommand : ICommand<Guid>
{
	public Guid? ProjectId { get; init; }
	public string? Description { get; init; }
	public string? Details { get; init; }
	public DateOnly? Date { get; init; }
	public TimeOnly? Start { get; init; }
	public TimeOnly? End { get; init; }
}

internal sealed class CreateTaskCommandHandler(DevBookDbContext dbContext, TimeProvider timeProvider) : ICommandHandler<CreateWorkTaskCommand, Guid>
{
	public async Task<Guid> Handle(CreateWorkTaskCommand request, CancellationToken cancellationToken)
	{
		var newItem = new WorkTask(
			ProjectId: request.ProjectId,
			Description: request.Description,
			Details: request.Details,
			Date: request.Date,
			Start: request.Start ?? TimeOnly.FromDateTime(timeProvider.GetLocalNow().DateTime),
			End: request.End);

		await dbContext.Tasks.AddAsync(newItem, cancellationToken);
		await dbContext.SaveChangesAsync(cancellationToken);
		return newItem.Id;
	}
}
