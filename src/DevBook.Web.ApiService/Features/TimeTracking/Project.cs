using DevBook.Web.Shared;
using System.Drawing;

namespace DevBook.Web.ApiService.Features.TimeTracking;

public sealed record Project(string Name, string? Details, int? HourlyRate, string? Currency, Color? Color = default)
	: Entity(Guid.NewGuid());
