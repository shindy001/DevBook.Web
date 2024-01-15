using DevBook.Web.Client.WASM.ApiClient;
using DevBook.WebApiClient.Generated;
using Microsoft.AspNetCore.Components.Authorization;
using OneOf;
using OneOf.Types;
using System.Net;
using System.Security.Claims;

namespace DevBook.Web.Client.WASM.Identity;

internal sealed class CookieAuthenticationStateProvider(
	IDevBookWebApiActionExecutor devBookWebApiActionExecutor)
	: AuthenticationStateProvider, IAccountManagement
{
	/// <summary>
	/// Authentication state.
	/// </summary>
	private bool _isAuthenticated = false;

	/// <summary>
	/// Default principal for anonymous (not authenticated) users.
	/// </summary>
	private readonly ClaimsPrincipal Unauthenticated = new(new ClaimsIdentity());

	/// <summary>
	/// Register a new user.
	/// </summary>
	/// <param name="email">The user's email address.</param>
	/// <param name="password">The user's password.</param>
	/// <returns>The result of the request serialized to <see cref="Success"/> or <see cref="ApiError"/>.</returns>
	public async Task<OneOf<Success, ApiError>> RegisterAsync(string email, string password)
	{
		return await devBookWebApiActionExecutor.Execute(x => x.RegisterAsync(
			new RegisterRequest { Email = email, Password = password }));
 	}

	/// <summary>
	/// User login.
	/// </summary>
	/// <param name="email">The user's email address.</param>
	/// <param name="password">The user's password.</param>
	/// <returns>The result of the request serialized to <see cref="Success"/> or <see cref="ApiError"/>.</returns>
	public async Task<OneOf<Success, ApiError>> LoginAsync(string email, string password)
	{
		var result = await devBookWebApiActionExecutor.Execute(x => x.LoginAsync(
			useCookies: true,
			useSessionCookies: false,
			body: new LoginRequest { Email = email, Password = password }));

		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

		// ApiError with StatusCode 200 OK => successful login
		// .net 8 Identity returns only 200 OK status when using cookies,
		// but generated client IDevBookWebApiClient throws ApiException because it expects AccessTokenResponse as specified in model
		return result.Match<OneOf<Success, ApiError>>(
			success => new Success(),
			apiError => 
				apiError.StatusCode is HttpStatusCode.OK
				? new Success()
				: apiError.StatusCode is HttpStatusCode.Unauthorized
					? new ApiError(apiError.StatusCode, ["Invalid email and/or password."])
					: apiError);
	}

	/// <summary>
	/// User logout
	/// </summary>
	/// <returns>The result of the request serialized to <see cref="Success"/> or <see cref="ApiError"/>.</returns>
	public async Task<OneOf<Success, ApiError>> LogoutAsync()
	{
		var result = await devBookWebApiActionExecutor.Execute(x => x.LogoutAsync());

		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

		return result;
	}

	/// <summary>
	/// Checks if user is authenticated
	/// </summary>
	/// <returns>true if authenticated</returns>
	public async Task<bool> CheckAuthenticatedAsync()
	{
		await GetAuthenticationStateAsync();
		return _isAuthenticated;
	}

	public async override Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		_isAuthenticated = false;
		var user = Unauthenticated;

		var result = await devBookWebApiActionExecutor.Execute(x => x.ManageInfoGETAsync());

		if (result.TryPickT0(out var userInfo, out _))
		{
			var claims = new List<Claim>
			{
				new (ClaimTypes.Name, userInfo?.Email ?? string.Empty),
				new (ClaimTypes.Email, userInfo?.Email ?? string.Empty)
			};
			var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
			user = new ClaimsPrincipal(id);
			_isAuthenticated = true;
		}

		return new AuthenticationState(user);
	}
}
