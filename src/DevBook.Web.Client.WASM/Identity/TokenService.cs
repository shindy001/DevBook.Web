using Blazored.LocalStorage;
using DevBook.Web.Client.WASM.ApiClient;
using DevBook.WebApiClient.Generated;

namespace DevBook.Web.Client.WASM.Identity;

internal interface ITokenService
{
	public Task<string> GetToken();
	public Task SetTokens(string? token, string? refreshToken);
	public Task RemoveTokens();
	public Task<bool> RefreshTokens();
}

internal sealed class TokenService(IDevBookWebApiClientFactory _devBookWebApiClientFactory, ILocalStorageService _localStorageService) : ITokenService
{
	private readonly IDevBookWebApiClient _devBookWebApiClient = _devBookWebApiClientFactory.Create();

	public async Task<string> GetToken()
	{
		return await _localStorageService.GetItemAsStringAsync(IdentityConstants.TokenName) ?? string.Empty;
	}

	public async Task SetTokens(string? token, string? refreshToken)
	{
		await _localStorageService.SetItemAsStringAsync(IdentityConstants.TokenName, token ?? string.Empty);
		await _localStorageService.SetItemAsStringAsync(IdentityConstants.RefreshTokenName, refreshToken ?? string.Empty);
	}

	public async Task<bool> RefreshTokens()
	{
		try
		{
			var refreshToken = await _localStorageService.GetItemAsStringAsync(IdentityConstants.RefreshTokenName);

			if (string.IsNullOrWhiteSpace(refreshToken))
			{
				return false;
			}

			var response = await _devBookWebApiClient.Identity_RefreshAsync(new RefreshRequest { RefreshToken = refreshToken });
			await SetTokens(response.AccessToken, response.RefreshToken);
			return true;
		}
		catch (ApiException)
		{
			return false;
		}
	}

	public async Task RemoveTokens()
	{
		await _localStorageService.RemoveItemsAsync([IdentityConstants.TokenName, IdentityConstants.RefreshTokenName]);
	}
}
