﻿using DevBook.Web.ApiService.Features.TimeTracking.Shared;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DevBook.Web.ApiService.Features.TimeTracking.Tasks;

public sealed record WorkTaskDto
{
	[Required]
	public Guid Id { get; init; }
	public Project? Project { get; init; }
	public string? Description { get; init; }
	public string? Details { get; init; }

	[Required]
	public DateTimeOffset Date { get; init; }

	[Required]
	public TimeOnly Start { get; init; }
	public TimeOnly? End { get; init; }

	public WorkTaskDto(Guid id, Project? project, string? description, string? details, DateTimeOffset date,	TimeOnly start,	TimeOnly? end)
	{
		Id = id;
		Project = project;
		Description = description;
		Details = details;
		Date = date;
		Start = start;
		End = end;
	}
}
