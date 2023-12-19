namespace DevBook.Web.Shared.Contracts;

/// <summary>
/// Represents an entity which handles execution of queries and commands
/// </summary>
public interface IExecutor
{
	Task<TResult> ExecuteQuery<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);

	Task<TResult> ExecuteCommand<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);

	Task ExecuteCommand(ICommand command, CancellationToken cancellationToken = default);
}
