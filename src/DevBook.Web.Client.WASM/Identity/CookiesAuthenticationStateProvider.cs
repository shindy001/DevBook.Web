using DevBook.WebApiClient.Generated;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Linq;

namespace DevBook.Web.Client.WASM.Identity;

public class CookieAuthenticationStateProvider(
	IDevBookWebApiClient devBookWebClient,
	ILogger<CookieAuthenticationStateProvider> logger)
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
	/// <returns>The result serialized to a <see cref="AccountActionResult"/>.
	/// </returns>
	public async Task<AccountActionResult> RegisterAsync(string email, string password)
	{
		try
		{
			await devBookWebClient.RegisterAsync(new RegisterRequest { Email = email, Password = password });

			return new AccountActionResult { Succeeded = true };

		}
		catch (ApiException ex) when (ex is ApiException<HttpValidationProblemDetails> problemDetails)
		{
			return new AccountActionResult
			{
				Succeeded = false,
				ErrorList = [.. problemDetails?.Result?.Errors?.Values.Select(x => string.Join(Environment.NewLine, x))]
			};
		}
		catch (Exception ex)
		{
			logger.LogTrace("Error while trying to Register: {ex}", ex);

			return new AccountActionResult
			{
				Succeeded = false,
				ErrorList = ["An unknown error prevented registration from succeeding."]
			};
		}
 	}

	/// <summary>
	/// User login.
	/// </summary>
	/// <param name="email">The user's email address.</param>
	/// <param name="password">The user's password.</param>
	/// <returns>The result of the login request serialized to a <see cref="AccountActionResult"/>.</returns>
	public async Task<AccountActionResult> LoginAsync(string email, string password)
	{
		try
		{
			await devBookWebClient.LoginAsync(
				useCookies: true,
				useSessionCookies: false,
			body: new LoginRequest { Email = email, Password = password });

			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

			return new AccountActionResult { Succeeded = true };
		}
		catch (ApiException ex) when (ex.StatusCode == 200)
		{
			// Successful login with 200 OK
			// .net 8 Identity actually returns only 200 OK status when using cookies, but generated client IDevBookWebApiClient throws ApiException because it expects AccessTokenResponse
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

			return new AccountActionResult { Succeeded = true };
		}
		catch (Exception ex)
		{
			logger.LogTrace("Error while trying to Login: {ex}", ex);

			return new AccountActionResult
			{
				Succeeded = false,
				ErrorList = ["Invalid email and/or password."]
			};
		}
	}

	public async Task LogoutAsync()
	{
		await devBookWebClient.LogoutAsync();

		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	}

	public async Task<bool> CheckAuthenticatedAsync()
	{
		await GetAuthenticationStateAsync();
		return _isAuthenticated;
	}

	public async override Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		_isAuthenticated = false;
		var user = Unauthenticated;

		try
		{
			var userResponse = await devBookWebClient.ManageInfoGETAsync();
			if (userResponse != null)
			{
				var claims = new List<Claim>
				{
					new (ClaimTypes.Name, userResponse?.Email ?? string.Empty),
					new (ClaimTypes.Email, userResponse?.Email ?? string.Empty)
				};
				var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
				user = new ClaimsPrincipal(id);
				_isAuthenticated = true;
			}
			return new AuthenticationState(user);
		}
		catch { return new AuthenticationState(user); }
	}
}
