using MediatR;

namespace DevBook.Web.Shared.Contracts;

public interface IQueryPipelineBehavior<TRequest, TResponse>
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : IQuery<TResponse>;
