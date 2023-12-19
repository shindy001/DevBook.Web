using DevBook.Web.Shared.Contracts;
using MediatR;

namespace DevBook.Web.Shared;
public sealed class Executor(ISender _mediator) : IExecutor
{
	public async Task<TResult> ExecuteQuery<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
		=> await _mediator.Send(query, cancellationToken);

	public async Task<TResult> ExecuteCommand<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
		=> await _mediator.Send(command, cancellationToken);

	public async Task ExecuteCommand(ICommand command, CancellationToken cancellationToken = default)
		=> await _mediator.Send(command, cancellationToken);
}
