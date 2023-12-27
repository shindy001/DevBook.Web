using DevBook.Web.Shared.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DevBook.Web.Shared.Extensions;

public static class ExecutorExtensions
{
	/// <summary>
	/// Registers <see cref="IExecutor"/> scoped service and discovered command/query handlers
	/// </summary>
	/// <param name="services"></param>
	/// <param name="commandAndQueriesAssemblies"></param>
	/// <returns></returns>
	public static IServiceCollection AddCommandsAndQueriesExecutor(this IServiceCollection services, params Assembly[] commandAndQueriesAssemblies)
	{
		services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(commandAndQueriesAssemblies));
		services.AddScoped<IExecutor, Executor>();

		return services;
	}

	/// <summary>
	/// Registeres passed behavior as generic scoped pipeline behavior <see cref="IPipelineBehavior{TRequest, TResponse}"/>
	/// </summary>
	/// <param name="services"></param>
	/// <param name="pipelineBehavior"></param>
	/// <returns></returns>
	public static IServiceCollection AddPipelineBehavior(this IServiceCollection services, Type pipelineBehavior)
	{
		services.AddScoped(typeof(IPipelineBehavior<,>), pipelineBehavior);

		return services;
	}
}
