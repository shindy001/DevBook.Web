using DevBook.WebApiClient.Generated;
using System.Net.Http.Headers;

namespace DevBook.Web.Client;

public interface IDevBookApiProvider
{
	IDevBookWebApiClient Client { get; }

	void SetBearerToken(string token);
}

public sealed class DevBookApiProvider(HttpClient httpClient) : IDevBookApiProvider
{
	public IDevBookWebApiClient Client { get; } = new DevBookWebApiClient(httpClient);

	public void SetBearerToken(string token)
	{
		httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
	}
}
