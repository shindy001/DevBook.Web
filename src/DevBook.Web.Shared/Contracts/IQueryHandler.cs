using MediatR;

namespace DevBook.Web.Shared.Contracts;

/// <summary>
/// Defines a handler for a query
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
	where TQuery : IQuery<TResponse>;
