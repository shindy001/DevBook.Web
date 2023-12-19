using MediatR;

namespace DevBook.Web.Shared.Contracts;

/// <summary>
/// Represent Query action with a response
/// </summary>
public interface IQuery<TResponse> : IRequest<TResponse>;
