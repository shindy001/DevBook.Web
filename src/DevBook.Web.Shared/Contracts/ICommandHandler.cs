using MediatR;

namespace DevBook.Web.Shared.Contracts;

/// <summary>
/// Defines a handler for a command with void response
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface ICommandHandler<TCommand> : IRequestHandler<TCommand>
	where TCommand : ICommand;

/// <summary>
/// Defines a handler for a command
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
	where TCommand : ICommand<TResponse>;
