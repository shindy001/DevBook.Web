using DevBook.WebApiClient.Generated;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace DevBook.Web.Client.Features.Account;

public class DevBookTokenAuthenticationStateProvider(
	ILogger<DevBookTokenAuthenticationStateProvider> logger,
	IDevBookApiProvider devBookWebApiProvider,
	IJSRuntime jsRuntime)
	: AuthenticationStateProvider
{
	public async Task<string> GetTokenAsync()
			=> await jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
	public async Task<string> GetRefreshTokenAsync()
			=> await jsRuntime.InvokeAsync<string>("localStorage.getItem", "authRefreshToken");

	public async Task SetTokenAsync(string? token, string? refreshToken)
	{
		if (token is null)
		{
			await jsRuntime.InvokeAsync<object>("localStorage.removeItem", "authToken");
			await jsRuntime.InvokeAsync<object>("localStorage.removeItem", "authRefreshToken");
		}
		else
		{
			await jsRuntime.InvokeAsync<object>("localStorage.setItem", "authToken", token);
			await jsRuntime.InvokeAsync<object>("localStorage.setItem", "authRefreshToken", refreshToken);
		}

		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	}

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		var token = await GetTokenAsync();
		var refreshToken = await GetRefreshTokenAsync();

		if (string.IsNullOrWhiteSpace(token))
		{
			return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
		}

		string? userName;
		try
		{
			var result = await GetUserNameOrRefreshTokens(devBookWebApiProvider.Client, token, refreshToken);
			userName = result.userName;
			if (result.tokensRefreshed)
			{
				var retryResult = await GetUserNameOrRefreshTokens(devBookWebApiProvider.Client, token, refreshToken);
				userName = retryResult.userName;
			}
		}
		catch (Exception ex)
		{
			logger.LogError("Authentication error: {ex}", ex);
			return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
		}

		return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(userName)));
	}

	private async Task<(string? userName, bool tokensRefreshed)> GetUserNameOrRefreshTokens(IDevBookWebApiClient client, string token, string refreshToken)
	{
		try
		{
			devBookWebApiProvider.SetBearerToken(token);
			var userData = await client.ManageInfoGETAsync();
			return (userName: userData.Email, tokensRefreshed: false);
		}
		catch (ApiException ex) when (ex.StatusCode is StatusCodes.Status401Unauthorized)
		{
			await RefreshTokens(devBookWebApiProvider.Client, refreshToken);
			return (userName: null, tokensRefreshed: true);
		}
	}

	private async Task RefreshTokens(IDevBookWebApiClient client, string refreshToken)
	{
		var response = await client.RefreshAsync(new RefreshRequest { RefreshToken = refreshToken });
		await SetTokenAsync(response.AccessToken, response.RefreshToken);
	}
}
