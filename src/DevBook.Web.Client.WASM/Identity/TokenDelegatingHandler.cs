using System.Net;

namespace DevBook.Web.Client.WASM.Identity;

/// <summary>
/// Handler to ensure cookie credentials are automatically sent over with each request.
/// </summary>
internal sealed class TokenDelegatingHandler(ITokenService _tokenService) : DelegatingHandler
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

		await SetAuthorizationHeader(request);
		var response = await base.SendAsync(request, cancellationToken);

		if (response.StatusCode is HttpStatusCode.Unauthorized && !_refreshingTokens)
		{
			try
			{
				_refreshingTokens = true;
				var tokensRefreshed = await _tokenService.RefreshTokens();
				if (tokensRefreshed)
				{
					await SetAuthorizationHeader(request);
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

	private async Task SetAuthorizationHeader(HttpRequestMessage request)
	{
		var token = await _tokenService.GetToken();
		request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
	}
}
