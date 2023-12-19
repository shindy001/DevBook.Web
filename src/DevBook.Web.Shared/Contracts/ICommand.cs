using MediatR;

namespace DevBook.Web.Shared.Contracts;

/// <summary>
/// Marker interface to be able to target only Commands (e.g. in pipeline behaviors),
/// MediatR IRequest is for both command and query by default
/// </summary>
public interface ICommandBase;

public interface ICommand : IRequest;

public interface ICommand<TResponse> : IRequest<TResponse>;
