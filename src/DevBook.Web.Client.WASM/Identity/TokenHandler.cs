using Blazored.LocalStorage;
using DevBook.WebApiClient.Generated;
using System.Net;

namespace DevBook.Web.Client.WASM.Identity;

/// <summary>
/// Handler to ensure cookie credentials are automatically sent over with each request.
/// </summary>
internal sealed class TokenHandler(ILocalStorageService _localStorageService, IHttpClientFactory _httpClientFactory) : DelegatingHandler
{
	private bool _refreshingTokens;

	/// <summary>
	/// Main method to override for the handler.
	/// </summary>
	/// <param name="request">The original request.</param>
	/// <param name="cancellationToken">The token to handle cancellations.</param>
	/// <returns>The <see cref="HttpResponseMessage"/>.</returns>
	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{

		await SetAuthorizationHeader(request, cancellationToken);
		var response = await base.SendAsync(request, cancellationToken);

		if (response.StatusCode is HttpStatusCode.Unauthorized && !_refreshingTokens)
		{
			try
			{
				// TODO - refactor, client should be injected here, will probably need special DevBookApiClient instance in DI for refreshing
				var client = _httpClientFactory.CreateClient();
				client.BaseAddress = new Uri("https://localhost:7126");
				var api = new DevBookWebApiClient(client);
				_refreshingTokens = true;
				var refreshToken = await _localStorageService.GetItemAsStringAsync(IdentityConstants.RefreshTokenName, cancellationToken);
				if (client is not null && (await api.Identity_RefreshAsync(new RefreshRequest { RefreshToken = refreshToken}, cancellationToken)) is AccessTokenResponse accessTokenResponse)
				{
					// TODO - extract to TokenService
					await _localStorageService.SetItemAsStringAsync(IdentityConstants.TokenName, accessTokenResponse.AccessToken ?? string.Empty);
					await _localStorageService.SetItemAsStringAsync(IdentityConstants.RefreshTokenName, accessTokenResponse.RefreshToken ?? string.Empty);
					await SetAuthorizationHeader(request, cancellationToken);
					response = await base.SendAsync(request, cancellationToken);
				}
			}
			finally
			{
				_refreshingTokens = false;
			}
		}

		return response;
	}

	private async Task SetAuthorizationHeader(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		// TODO - extract to TokenService
		var token = await _localStorageService.GetItemAsStringAsync(IdentityConstants.TokenName, cancellationToken);

		request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
	}
}
