using DevBook.Web.ApiService.Infrastructure;
using DevBook.Web.Shared.Contracts;
using FluentValidation;
using System.Drawing;

namespace DevBook.Web.ApiService.Features.TimeTracking;

public sealed record CreateProjectCommand(
	string Name,
	string? Details,
	int? HourlyRate,
	string? Currency,
	Color? Color) : ICommand<Guid>;

public sealed class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
	public CreateProjectCommandValidator()
	{
		RuleFor(x => x.Name).NotEmpty();
	}
}

internal sealed class CreateProjectCommandHandler(DevBookDbContext dbContext) : ICommandHandler<CreateProjectCommand, Guid>
{
	public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
	{
		var newItem = new Project(request.Name, request.Details, request.HourlyRate, request.Currency, request.Color);
		await dbContext.Projects.AddAsync(newItem, cancellationToken);
		await dbContext.SaveChangesAsync(cancellationToken);
		return newItem.Id;
	}
}
