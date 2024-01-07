using DevBook.WebApiClient.Generated;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace DevBook.Web.Client.WASM;

public class CookieAuthenticationStateProvider(
	IDevBookWebApiClient devBookWebClient,
	ILogger<CookieAuthenticationStateProvider> logger)
	: AuthenticationStateProvider
{
	public async override Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		var user = new ClaimsPrincipal(new ClaimsIdentity());

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
			}
			return new AuthenticationState(user);
		}
		catch { return new AuthenticationState(user); }
	}

	public async Task Login(LoginModel model)
	{
		try
		{
			await devBookWebClient.LoginAsync(
				useCookies: true,
				useSessionCookies: false,
				body: new LoginRequest { Email = model.Email, Password = model.Password });

		}
		catch (Exception ex)
		{
			logger.LogError("Error while trying to Login: {ex}", ex);
		}

		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	}
}

public class UserInfo
{
	public required string Email { get; set; }
}

public class LoginModel
{
	public string? Email { get; set; }
	public string? Password { get; set; }
}
