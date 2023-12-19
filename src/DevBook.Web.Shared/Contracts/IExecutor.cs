namespace DevBook.Web.Shared.Contracts;

public interface IExecutor
{
	Task<TResult> ExecuteQuery<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);

	Task<TResult> ExecuteCommand<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);

	Task ExecuteCommand(ICommand command, CancellationToken cancellationToken = default);
}
