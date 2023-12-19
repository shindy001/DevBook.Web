using MediatR;

namespace DevBook.Web.Shared.Contracts;

/// <summary>
/// Pipeline behavior to surround the inner command handler.
/// Implementations add additional behavior and await the next delegate.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface ICommandPipelineBehavior<TCommand, TResult>
	: IPipelineBehavior<TCommand, TResult>
	where TCommand : ICommandBase;
