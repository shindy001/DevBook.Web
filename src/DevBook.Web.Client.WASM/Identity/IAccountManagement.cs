namespace DevBook.Web.Client.WASM.Identity;

/// <summary>
/// Account management services.
/// </summary>
public interface IAccountManagement
{
	/// <summary>
	/// Login service.
	/// </summary>
	/// <param name="email">User's email.</param>
	/// <param name="password">User's password.</param>
	/// <returns>The result of the request serialized to <see cref="AccountActionResult"/>.</returns>
	public Task<AccountActionResult> LoginAsync(string email, string password);

	/// <summary>
	/// Log out the logged in user.
	/// </summary>
	/// <returns>The asynchronous task.</returns>
	public Task LogoutAsync();

	/// <summary>
	/// Registration service.
	/// </summary>
	/// <param name="email">User's email.</param>
	/// <param name="password">User's password.</param>
	/// <returns>The result of the request serialized to <see cref="AccountActionResult"/>.</returns>
	public Task<AccountActionResult> RegisterAsync(string email, string password);

	public Task<bool> CheckAuthenticatedAsync();
}
