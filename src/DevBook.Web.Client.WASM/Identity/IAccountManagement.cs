namespace DevBook.Web.Client.WASM.Identity;

/// <summary>
/// Account management services.
/// </summary>
public interface IAccountManagement
{
	/// <summary>
	/// Registration service.
	/// </summary>
	/// <param name="email">User's email.</param>
	/// <param name="password">User's password.</param>
	/// <returns>The result of the request serialized to <see cref="Success"/> or <see cref="ApiError"/>.</returns>
	public Task<OneOf<Success, ApiError>> RegisterAsync(string email, string password);

	/// <summary>
	/// Login service.
	/// </summary>
	/// <param name="email">User's email.</param>
	/// <param name="password">User's password.</param>
	/// <returns>The result of the request serialized to <see cref="Success"/> or <see cref="ApiError"/>.</returns>
	public Task<OneOf<Success, ApiError>> LoginAsync(string email, string password);

	/// <summary>
	/// Log out the logged in user.
	/// </summary>
	public Task LogoutAsync();

	/// <summary>
	/// Checks if user is authenticated
	/// </summary>
	/// <returns>true if authenticated</returns>
	public Task<bool> CheckAuthenticatedAsync();
}
