using DevBook.Web.ApiService.Infrastructure;
using DevBook.Web.Shared.Contracts;
using FluentValidation;
using OneOf;
using OneOf.Types;
using System.Drawing;

namespace DevBook.Web.ApiService.Features.TimeTracking;

public record UpdateProjectCommand(
	Guid Id,
	string Name,
	string? Details,
	int? HourlyRate,
	string? Currency,
	Color? Color)
	: ICommand<OneOf<Success, NotFound>>;

public sealed class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
	public UpdateProjectCommandValidator()
	{
		RuleFor(x => x.Id).NotEqual(Guid.Empty);
		RuleFor(x => x.Name).NotEmpty();
	}
}

public sealed class UpdateProjectCommandHandler(DevBookDbContext dbContext) : ICommandHandler<UpdateProjectCommand, OneOf<Success, NotFound>>
{
	public async Task<OneOf<Success, NotFound>> Handle(UpdateProjectCommand command, CancellationToken cancellationToken)
	{
		var project = await dbContext.Projects.FindAsync([command.Id], cancellationToken);
		if (project is null)
		{
			return new NotFound();
		}
		else
		{
			var update = new Dictionary<string, object?>()
			{
				[nameof(Project.Name)] = command.Name,
				[nameof(Project.Details)] = command.Details,
				[nameof(Project.HourlyRate)] = command.HourlyRate,
				[nameof(Project.Currency)] = command.Currency,
				[nameof(Project.Color)] = command.Color,
			};

			dbContext.Projects.Entry(project).CurrentValues.SetValues(update);
			await dbContext.SaveChangesAsync(cancellationToken);
			return new Success();
		}
	}
}
