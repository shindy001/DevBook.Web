using DevBook.Web.ApiService.Infrastructure;
using DevBook.Web.Shared.Contracts;
using OneOf;
using OneOf.Types;
using System.ComponentModel.DataAnnotations;

namespace DevBook.Web.ApiService.Features.TimeTracking.Tasks;

internal record UpdateWorkTaskCommandDto
{
	public Guid? ProjectId { get; init; }
	public string? Description { get; init; }
	public string? Details { get; init; }
	public DateOnly? Date { get; init; }

	[Required]
	public required TimeOnly Start { get; init; }
	public TimeOnly? End { get; init; }
}

internal record UpdateWorkTaskCommand(
	Guid Id,
	Guid? ProjectId,
	string? Description,
	string? Details,
	DateOnly? Date,
	TimeOnly Start,
	TimeOnly? End)
	: ICommand<OneOf<Success, NotFound>>;

internal sealed class UpdateUpdateWorkTaskCommandHandler(DevBookDbContext dbContext) : ICommandHandler<UpdateWorkTaskCommand, OneOf<Success, NotFound>>
{
	public async Task<OneOf<Success, NotFound>> Handle(UpdateWorkTaskCommand command, CancellationToken cancellationToken)
	{
		var workTask = await dbContext.Tasks.FindAsync([command.Id], cancellationToken);
		if (workTask is null)
		{
			return new NotFound();
		}
		else
		{
			var update = new Dictionary<string, object?>()
			{
				[nameof(WorkTask.ProjectId)] = command.ProjectId,
				[nameof(WorkTask.Description)] = command.Description,
				[nameof(WorkTask.Details)] = command.Details,
				[nameof(WorkTask.Date)] = command.Date,
				[nameof(WorkTask.Start)] = command.Start,
				[nameof(WorkTask.End)] = command.End,
			};

			dbContext.Tasks.Entry(workTask).CurrentValues.SetValues(update);
			await dbContext.SaveChangesAsync(cancellationToken);
			return new Success();
		}
	}
}
