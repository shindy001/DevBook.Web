﻿using MediatR;

namespace DevBook.Web.Shared.Contracts;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
	where TQuery : IQuery<TResponse>;
