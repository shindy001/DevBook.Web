using MediatR;

namespace DevBook.Web.Shared.Contracts;

/// <summary>
/// Marker interface to be able to target only Commands (e.g. in pipeline behaviors),
/// MediatR IRequest is for both command and query by default
/// </summary>
public interface ICommandBase;

/// <summary>
/// Represent Command action that does not return any value
/// </summary>
public interface ICommand : IRequest;

/// <summary>
/// Represents Command action that returns a result
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface ICommand<TResult> : IRequest<TResult>;
