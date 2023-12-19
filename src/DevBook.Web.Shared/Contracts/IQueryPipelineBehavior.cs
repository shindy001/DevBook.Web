using MediatR;

namespace DevBook.Web.Shared.Contracts;

/// <summary>
/// Pipeline behavior to surround the inner query handler.
/// Implementations add additional behavior and await the next delegate.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IQueryPipelineBehavior<TRequest, TResponse>
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : IQuery<TResponse>;
