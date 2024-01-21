using DevBook.Web.Client.WASM.ApiClient;
using DevBook.Web.Client.WASM.Exceptions;
using DevBook.WebApiClient.Generated;

namespace DevBook.Web.Client.WASM.Features.TimeTracking.Tasks;

public interface ITasksService
{
	Task<IEnumerable<WorkTaskDto>> GetAll();
	Task<WorkTaskDto> GetById(Guid id);
	Task Create(TimeOnly start, Guid? projectId = null, string? description = null, string? details = null, DateOnly? date = null, TimeOnly? end = null);
	Task Update(Guid id, TimeOnly start, Guid? projectId = null, string? description = null, string? details = null, DateOnly? date = null, TimeOnly? end = null);
	Task Patch(Guid id, TimeOnly? start, Guid? projectId = null, string? description = null, string? details = null, DateOnly? date = null, TimeOnly? end = null);
	Task Delete(Guid id);
}

internal sealed class TasksService(IDevBookWebApiActionExecutor devBookWebApiActionExecutor) : ITasksService
{
	public async Task<IEnumerable<WorkTaskDto>> GetAll()
	{
		var result = await devBookWebApiActionExecutor.Execute(x => x.WorkTasks_GetAllAsync());
		return result.Match(
			tasks => tasks,
			apiError => throw new DevBookException(apiError.Errors));
	}

	public async Task<WorkTaskDto> GetById(Guid id)
	{
		var result = await devBookWebApiActionExecutor.Execute(x => x.WorkTasks_GetByIdAsync(id));
		return result.Match(
			task => task,
			apiError => throw new DevBookException(apiError.Errors));
	}

	public async Task Create(TimeOnly start, Guid? projectId = null, string? description = null, string? details = null, DateOnly? date = null, TimeOnly? end = null)
	{
		var result = await devBookWebApiActionExecutor.Execute(x => x.WorkTasks_CreateAsync(
			new CreateWorkTaskCommand
			{
				Start = start.ToTimeSpan(),
				ProjectId = projectId,
				Description = description,
				Details = details,
				Date = date?.ToDateTime(TimeOnly.MinValue),
				End = end?.ToTimeSpan(),
			}));

		result.Match(
			success => success,
			apiError => throw new DevBookException(apiError.Errors));
	}

	public async Task Update(Guid id, TimeOnly start, Guid? projectId = null, string? description = null, string? details = null, DateOnly? date = null, TimeOnly? end = null)
	{
		var result = await devBookWebApiActionExecutor.Execute(x => x.WorkTasks_UpdateAsync(
			id,
			new UpdateWorkTaskCommandDto
			{
				Start = start.ToTimeSpan(),
				ProjectId = projectId,
				Description = description,
				Details = details,
				Date = date?.ToDateTime(TimeOnly.MinValue),
				End = end?.ToTimeSpan(),
			}));

		result.Match(
			success => success,
			apiError => throw new DevBookException(apiError.Errors));
	}

	public async Task Patch(Guid id, TimeOnly? start, Guid? projectId = null, string? description = null, string? details = null, DateOnly? date = null, TimeOnly? end = null)
	{
		var result = await devBookWebApiActionExecutor.Execute(x => x.WorkTasks_PatchAsync(
			id,
			new PatchWorkTaskCommandDto
			{
				Start = start?.ToTimeSpan(),
				ProjectId = projectId,
				Description = description,
				Details = details,
				Date = date?.ToDateTime(TimeOnly.MinValue),
				End = end?.ToTimeSpan(),
			}));

		result.Match(
			success => success,
			apiError => throw new DevBookException(apiError.Errors));
	}

	public async Task Delete(Guid id)
	{
		var result = await devBookWebApiActionExecutor.Execute(x => x.WorkTasks_DeleteAsync(id));

		result.Match(
			success => success,
			apiError => throw new DevBookException(apiError.Errors));
	}
}
