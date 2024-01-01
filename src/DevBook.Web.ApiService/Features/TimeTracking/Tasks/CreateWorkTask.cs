using DevBook.Web.ApiService.Infrastructure;
using DevBook.Web.Shared.Contracts;
using FluentValidation;

namespace DevBook.Web.ApiService.Features.TimeTracking.Tasks;

public sealed record CreateWorkTaskCommand : ICommand<Guid>
{
	public Guid? ProjectId { get; init; }
	public string? Description { get; init; }
	public string? Details { get; init; }
	public DateOnly? Date { get; init; }
	public TimeOnly? Start { get; init; }
	public TimeOnly? End { get; init; }
}

public sealed class CreateWorkTaskCommandValidator : AbstractValidator<CreateWorkTaskCommand>
{
	public CreateWorkTaskCommandValidator()
	{
		When(x => x.ProjectId is not null, () => RuleFor(x => x.ProjectId).NotEqual(Guid.Empty));
	}
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
