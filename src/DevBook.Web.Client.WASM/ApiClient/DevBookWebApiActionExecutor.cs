using DevBook.WebApiClient.Generated;
using OneOf;
using OneOf.Types;
using System.Net;

namespace DevBook.Web.Client.WASM.ApiClient;

internal interface IDevBookWebApiActionExecutor
{
	Task<OneOf<Success, ApiError>> Execute(Func<IDevBookWebApiClient, Task> method);
	Task<OneOf<TResult, ApiError>> Execute<TResult>(Func<IDevBookWebApiClient, Task<TResult>> method);
}

internal class DevBookWebApiActionExecutor(IDevBookWebApiClient devBookWebApiClient, ILogger<DevBookWebApiActionExecutor> logger) : IDevBookWebApiActionExecutor
{
	public async Task<OneOf<Success, ApiError>> Execute(Func<IDevBookWebApiClient, Task> method)
	{
		try
		{
			await method(devBookWebApiClient);
			return new Success();
		}
		catch (ApiException ex) when (ex is ApiException<HttpValidationProblemDetails> problemDetails)
		{
			return LogAndCreateApiError(nameof(method), (HttpStatusCode)ex.StatusCode, ex, problemDetails);
		}
		catch (ApiException ex)
		{
			return LogAndCreateApiError(nameof(method), (HttpStatusCode)ex.StatusCode, ex);
		}
		catch (HttpRequestException ex) when (ex.Message.Equals("TypeError: Failed to fetch")) // WASM js lib error
		{
			return LogAndCreateApiError(nameof(method), ex.StatusCode ?? HttpStatusCode.InternalServerError, ex);
		}
	}

	public async Task<OneOf<TResult, ApiError>> Execute<TResult>(Func<IDevBookWebApiClient, Task<TResult>> method)
	{
		try
		{
			return await method(devBookWebApiClient);
		}
		catch (ApiException ex) when (ex is ApiException<HttpValidationProblemDetails> problemDetails)
		{
			return LogAndCreateApiError(nameof(method), (HttpStatusCode)ex.StatusCode, ex, problemDetails);
		}
		catch (ApiException ex)
		{
			return LogAndCreateApiError(nameof(method), (HttpStatusCode)ex.StatusCode, ex);
		}
		catch (HttpRequestException ex) when (ex.Message.Equals("TypeError: Failed to fetch")) // WASM js lib error
		{
			return LogAndCreateApiError(nameof(method), ex.StatusCode ?? HttpStatusCode.InternalServerError, ex);
		}
	}

	private ApiError LogAndCreateApiError(string methodName, HttpStatusCode statusCode, Exception ex, ApiException<HttpValidationProblemDetails>? problemDetails = null)
	{
		logger.LogTrace("DevBookWebApi error when executing action {actionName}, {message}, {error}", methodName, ex.Message, ex);

		return new ApiError(statusCode, problemDetails?.Result?.Errors?.Values.Select(x => string.Join(Environment.NewLine, x)).ToArray() ?? [ex.Message]);
	}
}
