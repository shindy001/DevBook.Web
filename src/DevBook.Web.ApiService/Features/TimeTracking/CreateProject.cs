using DevBook.Web.ApiService.Infrastructure;
using DevBook.Web.Shared.Contracts;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace DevBook.Web.ApiService.Features.TimeTracking;

internal record CreateProjectResponse(Guid Id);

public sealed record CreateProjectCommand : ICommand<CreateProjectResponse>
{
	[Required]
	public required string Name { get; init; }
	public string? Details { get; init; }
	public int? HourlyRate { get; init; }
	public string? Currency { get; init; }
	public string? HexColor { get; init; }
}

public sealed class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
	public CreateProjectCommandValidator()
	{
		RuleFor(x => x.Name).NotEmpty();
	}
}

internal sealed class CreateProjectCommandHandler(DevBookDbContext dbContext) : ICommandHandler<CreateProjectCommand, CreateProjectResponse>
{
	public async Task<CreateProjectResponse> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
	{
		var newItem = new Project(request.Name, request.Details, request.HourlyRate, request.Currency, request.HexColor);
		await dbContext.Projects.AddAsync(newItem, cancellationToken);
		await dbContext.SaveChangesAsync(cancellationToken);
		return new CreateProjectResponse(newItem.Id);
	}
}
