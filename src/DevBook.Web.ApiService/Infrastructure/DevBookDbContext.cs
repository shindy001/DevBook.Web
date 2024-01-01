using DevBook.Web.ApiService.Features.TimeTracking.Projects;
using DevBook.Web.ApiService.Features.TimeTracking.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DevBook.Web.ApiService.Infrastructure;

public sealed class DevBookDbContext(DbContextOptions<DevBookDbContext> _options) : DbContext(_options)
{
	public DbSet<Project> Projects { get; set; }

	public DbSet<WorkTask> Tasks { get; set; }
}
