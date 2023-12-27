﻿using DevBook.Web.ApiService.Infrastructure;
using DevBook.Web.Shared.Contracts;
using FluentValidation;
using OneOf;
using OneOf.Types;
using System.Drawing;

namespace DevBook.Web.ApiService.Features.TimeTracking;

public record PatchProjectCommand(
	Guid Id,
	string? Name,
	string? Details,
	int? HourlyRate,
	string? Currency,
	Color? Color)
	: ICommand<OneOf<Success, NotFound>>;

public sealed class PatchProjectCommandValidator : AbstractValidator<PatchProjectCommand>
{
	public PatchProjectCommandValidator()
	{
		RuleFor(x => x.Id).NotEqual(Guid.Empty);
	}
}

public sealed class PatchProjectCommandHandler(DevBookDbContext dbContext) : ICommandHandler<PatchProjectCommand, OneOf<Success, NotFound>>
{
	public async Task<OneOf<Success, NotFound>> Handle(PatchProjectCommand command, CancellationToken cancellationToken)
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
				[nameof(Project.Name)] = command.Name ?? project.Name,
				[nameof(Project.Details)] = command.Details ?? project.Details,
				[nameof(Project.HourlyRate)] = command.HourlyRate ?? project.HourlyRate,
				[nameof(Project.Currency)] = command.Currency ?? project.Currency,
				[nameof(Project.Color)] = command.Color ?? project.Color,
			};

			dbContext.Projects.Entry(project).CurrentValues.SetValues(update);
			await dbContext.SaveChangesAsync(cancellationToken);
			return new Success();
		}
	}
}