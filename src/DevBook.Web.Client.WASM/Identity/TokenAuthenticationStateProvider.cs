using DevBook.Web.Client.WASM.ApiClient;
using DevBook.WebApiClient.Generated;
using Microsoft.AspNetCore.Components.Authorization;
using OneOf;
using OneOf.Types;
using System.Net;
using System.Security.Claims;

namespace DevBook.Web.Client.WASM.Identity;

internal sealed class TokenAuthenticationStateProvider(
	IDevBookWebApiActionExecutor _devBookWebApiActionExecutor,
	ITokenService _tokenService)
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
		return await _devBookWebApiActionExecutor.Execute(x => x.Identity_RegisterAsync(
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
		var result = await _devBookWebApiActionExecutor.Execute(x => x.Identity_LoginAsync(
			useCookies: false,
			useSessionCookies: false,
			body: new LoginRequest { Email = email, Password = password }));

		if (result.IsT0 && result.AsT0 is AccessTokenResponse tokenResponse && tokenResponse is not null)
		{
			await _tokenService.SetTokens(tokenResponse.AccessToken, tokenResponse.RefreshToken);
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
			return new Success();
		}

		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

		ApiError apiError = result.AsT1;
		return apiError.StatusCode is HttpStatusCode.Unauthorized
				? new ApiError(apiError.StatusCode, ["Invalid email and/or password."])
				: apiError;
	}

	/// <summary>
	/// User logout - removes token and refreshToken from local storage
	/// </summary>
	public async Task LogoutAsync()
	{
		await _tokenService.RemoveTokens();
		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
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
		var result = await _devBookWebApiActionExecutor.Execute(x => x.Identity_ManageInfoGETAsync());
		InfoResponse? userInfo = result.IsT0 ? result.AsT0 : null;

		if (result.IsT1 && result.AsT1 is ApiError apiError && apiError.StatusCode is HttpStatusCode.Unauthorized)
		{
			_ = await _tokenService.RefreshTokens();
		}

		return GetAuthenticationState(userInfo);
	}

	private AuthenticationState GetAuthenticationState(InfoResponse? userInfo)
	{
		_isAuthenticated = false;

		if (userInfo is null)
		{
			return new AuthenticationState(Unauthenticated);
		}

		var claims = new List<Claim>
		{
			new (ClaimTypes.Name, userInfo?.Email ?? string.Empty),
			new (ClaimTypes.Email, userInfo?.Email ?? string.Empty)
		};

		var id = new ClaimsIdentity(claims, nameof(TokenAuthenticationStateProvider));
		var user = new ClaimsPrincipal(id);
		_isAuthenticated = !string.IsNullOrWhiteSpace(userInfo?.Email);

		return new AuthenticationState(user);
	}
}
