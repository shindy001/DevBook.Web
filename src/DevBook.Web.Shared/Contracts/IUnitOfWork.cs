namespace DevBook.Web.Shared.Contracts;

/// <summary>
/// Represents database unit of work
/// </summary>
public interface IUnitOfWork
{
	/// <summary>
	/// Changes will not be tracked hence cannot be commited after calling this 
	/// </summary>
	void AsNoTrackingQuery();

	/// <summary>
	/// Commits changes to DB
	/// </summary>
	/// <returns>
	/// The number of state entries written to the database.
	/// </returns>
	/// <exception cref="InvalidOperationException">
	/// An error is encountered when trying to commit changes and <see cref="AsNoTrackingQuery"/> was called before.
	/// </exception>
	Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
