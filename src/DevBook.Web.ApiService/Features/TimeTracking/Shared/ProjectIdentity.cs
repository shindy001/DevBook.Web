namespace DevBook.Web.ApiService.Features.TimeTracking.Shared;

public sealed record ProjectIdentity(Guid Id, string Name);

public static class ProjectIdentityExtensions
{
	public static ProjectIdentity Identity(this Project project) => new ProjectIdentity(project.Id, project.Name);
}
