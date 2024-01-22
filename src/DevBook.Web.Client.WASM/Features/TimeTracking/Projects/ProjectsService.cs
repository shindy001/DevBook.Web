﻿using DevBook.Web.Client.WASM.ApiClient;
using DevBook.Web.Client.WASM.Exceptions;
using DevBook.WebApiClient.Generated;

namespace DevBook.Web.Client.WASM.Features.TimeTracking.Projects;

public interface IProjectsService
{
	Task<IEnumerable<ProjectDto>> GetAll();
	Task<ProjectDto> GetById(Guid id);
	Task Create(string name, string? details = null, string? currency = null, int? hourlyRate = null, string? hexColor = null);
	Task Update(Guid id, string name, string? details = null, string? currency = null, int? hourlyRate = null, string? hexColor = null);
	Task Patch(Guid id, string? name = null, string? details = null, string? currency = null, int? hourlyRate = null, string? hexColor = null);
	Task Delete(Guid id);
}

internal sealed class ProjectsService(IDevBookWebApiActionExecutor devBookWebApiActionExecutor) : IProjectsService
{
	public async Task<IEnumerable<ProjectDto>> GetAll()
	{
		var result = await devBookWebApiActionExecutor.Execute(x => x.Projects_GetAllAsync());
		return result.Match(
			projects => projects,
			apiError => throw new DevBookException(apiError.Errors));
	}

	public async Task<ProjectDto> GetById(Guid id)
	{
		var result = await devBookWebApiActionExecutor.Execute(x => x.Projects_GetByIdAsync(id));
		return result.Match(
			project => project,
			apiError => throw new DevBookException(apiError.Errors));
	}

	public async Task Create(string name, string? details = null, string? currency = null, int? hourlyRate = null, string? hexColor = null)
	{
		var result = await devBookWebApiActionExecutor.Execute(x => x.Projects_CreateAsync(
			new CreateProjectCommand
			{
				Name = name,
				Details = details,
				Currency = currency,
				HourlyRate = hourlyRate,
				HexColor = hexColor
			}));

		result.Match(
			success => success,
			apiError => throw new DevBookException(apiError.Errors));
	}

	public async Task Update(Guid id, string name, string? details = null, string? currency = null, int? hourlyRate = null, string? hexColor = null)
	{
		var result = await devBookWebApiActionExecutor.Execute(x => x.Projects_UpdateAsync(
			id,
			new UpdateProjectCommandDto
			{
				Name = name,
				Details = details,
				Currency = currency,
				HourlyRate = hourlyRate,
				HexColor = hexColor
			}));

		result.Match(
			success => success,
			apiError => throw new DevBookException(apiError.Errors));
	}

	public async Task Patch(Guid id, string? name = null, string? details = null, string? currency = null, int? hourlyRate = null, string? hexColor = null)
	{
		var result = await devBookWebApiActionExecutor.Execute(x => x.Projects_PatchAsync(
			id,
			new PatchProjectCommandDto
			{
				Name = name,
				Details = details,
				Currency = currency,
				HourlyRate = hourlyRate,
				HexColor = hexColor
			}));

		result.Match(
			success => success,
			apiError => throw new DevBookException(apiError.Errors));
	}

	public async Task Delete(Guid id)
	{
		var result = await devBookWebApiActionExecutor.Execute(x => x.Projects_DeleteAsync(id));

		result.Match(
			success => success,
			apiError => throw new DevBookException(apiError.Errors));
	}
}
