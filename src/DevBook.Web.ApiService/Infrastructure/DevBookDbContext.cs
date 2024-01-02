using DevBook.Web.ApiService.Features.TimeTracking.Projects;
using DevBook.Web.ApiService.Features.TimeTracking.Tasks;
using DevBook.Web.ApiService.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevBook.Web.ApiService.Infrastructure;

internal sealed class DevBookDbContext(DbContextOptions<DevBookDbContext> options) : IdentityDbContext<DevBookUser>(options)
{
	public DbSet<Project> Projects { get; set; }

	public DbSet<WorkTask> Tasks { get; set; }
}
