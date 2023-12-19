using DevBook.Web.Shared.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DevBook.Web.Shared;

public static class ExecutorDependencyInjection
{
	public static IServiceCollection AddCommandsAndQueriesExecutor(this IServiceCollection services, params Assembly[] commandAndQueriesAssemblies)
	{
		services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(commandAndQueriesAssemblies));
		services.AddScoped<IExecutor, Executor>();

		return services;
	}

	public static IServiceCollection AddPipelineBehavior(this IServiceCollection services, Type pipelineBehavior)
	{
		services.AddScoped(typeof(IPipelineBehavior<,>), pipelineBehavior);

		return services;
	}
}
