
using DevBook.WebApiClient.Generated;

namespace DevBook.Web.Client.WASM.ApiClient;

internal interface IDevBookWebApiClientFactory
{
	IDevBookWebApiClient Create();
}

internal sealed class DevBookWebApiClientFactory(HttpClient _httpClient) : IDevBookWebApiClientFactory
{
	public IDevBookWebApiClient Create()
	{
		return new DevBookWebApiClient(_httpClient);
	}
}
