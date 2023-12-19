using MediatR;

namespace DevBook.Web.Shared.Contracts;
public interface ICommandPipelineBehavior<TRequest, TResponse>
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : ICommandBase;
