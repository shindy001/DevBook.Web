using MediatR;

namespace DevBook.Web.Shared.Contracts;

public interface IQuery<TResponse> : IRequest<TResponse>;
