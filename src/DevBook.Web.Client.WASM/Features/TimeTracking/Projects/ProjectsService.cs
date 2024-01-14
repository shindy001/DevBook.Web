using DevBook.Web.Client.WASM.ApiClient;
using DevBook.WebApiClient.Generated;
using OneOf;
using OneOf.Types;

namespace DevBook.Web.Client.WASM.Features.TimeTracking.Projects;

public interface IProjectsService;

internal sealed class ProjectsService(IDevBookWebApiActionExecutor devBookWebApiActionExecutor) : IProjectsService
{
	public async Task<OneOf<ICollection<Project>, ApiError>> GetAll()
	{
		return await devBookWebApiActionExecutor.Execute(x => x.ProjectsAllAsync());
	}

	public async Task<OneOf<Project, ApiError>> GetById(Guid id)
	{
		return await devBookWebApiActionExecutor.Execute(x => x.GetProjectByIdAsync(id));
	}

	public async Task<OneOf<Success, ApiError>> Create(string name, string? details = null, string? currency = null, int? hourlyRate = null, string? hexColor = null)
	{
		return await devBookWebApiActionExecutor.Execute(x => x.ProjectsPOSTAsync(
			new CreateProjectCommand
			{
				Name = name,
				Details = details,
				Currency = currency,
				HourlyRate = hourlyRate,
				HexColor = hexColor
			}));
	}

	public async Task<OneOf<Success, ApiError>> Update(Guid id, string name, string? details = null, string? currency = null, int? hourlyRate = null, string? hexColor = null)
	{
		return await devBookWebApiActionExecutor.Execute(x => x.ProjectsPUTAsync(
			id,
			new UpdateProjectCommandDto
			{
				Name = name,
				Details = details,
				Currency = currency,
				HourlyRate = hourlyRate,
				HexColor = hexColor
			}));
	}

	public async Task<OneOf<Success, ApiError>> Patch(Guid id, string? name = null, string? details = null, string? currency = null, int? hourlyRate = null, string? hexColor = null)
	{
		return await devBookWebApiActionExecutor.Execute(x => x.ProjectsPATCHAsync(
			id,
			new PatchProjectCommandDto
			{
				Name = name,
				Details = details,
				Currency = currency,
				HourlyRate = hourlyRate,
				HexColor = hexColor
			}));
	}

	public async Task<OneOf<Success, ApiError>> Delete(Guid id)
	{
		return await devBookWebApiActionExecutor.Execute(x => x.ProjectsDELETEAsync(id));
	}
}
