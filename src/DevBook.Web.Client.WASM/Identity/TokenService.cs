using Blazored.LocalStorage;
using DevBook.Web.Client.WASM.ApiClient;
using DevBook.WebApiClient.Generated;

namespace DevBook.Web.Client.WASM.Identity;

internal interface ITokenService
{
	public Task<string> GetToken();
	public Task SetTokens(string? token, string? refreshToken, long tokenExpiresInSeconds);
	public Task RemoveTokens();
	public Task<bool> RefreshTokens();
	Task<bool> IsTokenValid();
}

internal sealed class TokenService(
	IDevBookWebApiClientFactory _devBookWebApiClientFactory,
	ILocalStorageService _localStorageService,
	TimeProvider _timeProvider)
	: ITokenService
{
	private readonly IDevBookWebApiClient _devBookWebApiClient = _devBookWebApiClientFactory.Create();

	public async Task<string> GetToken()
	{
		return await _localStorageService.GetItemAsStringAsync(IdentityConstants.Token) ?? string.Empty;
	}

	public async Task SetTokens(string? token, string? refreshToken, long tokenExpiresInSeconds)
	{
		var expiresAt = _timeProvider.GetUtcNow().AddSeconds(tokenExpiresInSeconds);
		await _localStorageService.SetItemAsStringAsync(IdentityConstants.Token, token ?? string.Empty);
		await _localStorageService.SetItemAsStringAsync(IdentityConstants.RefreshToken, refreshToken ?? string.Empty);
		await _localStorageService.SetItemAsStringAsync(IdentityConstants.TokenExpireAt, expiresAt.ToString("o"));
	}

	public async Task RemoveTokens()
	{
		await _localStorageService.RemoveItemsAsync([IdentityConstants.Token, IdentityConstants.RefreshToken, IdentityConstants.TokenExpireAt]);
	}

	public async Task<bool> RefreshTokens()
	{
		try
		{
			var refreshToken = await _localStorageService.GetItemAsStringAsync(IdentityConstants.RefreshToken);

			if (string.IsNullOrWhiteSpace(refreshToken))
			{
				return false;
			}

			var response = await _devBookWebApiClient.Identity_RefreshAsync(new RefreshRequest { RefreshToken = refreshToken });
			await SetTokens(response.AccessToken, response.RefreshToken, response.ExpiresIn);
			return true;
		}
		catch (ApiException)
		{
			return false;
		}
	}

	public async Task<bool> IsTokenValid()
	{
		var token = await _localStorageService.GetItemAsStringAsync(IdentityConstants.TokenExpireAt);
		return !string.IsNullOrWhiteSpace(token) && DateTimeOffset.Parse(token) > _timeProvider.GetUtcNow();
	}
}
