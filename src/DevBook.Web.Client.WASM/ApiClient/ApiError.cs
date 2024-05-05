namespace DevBook.Web.Client.WASM.ApiClient;

public readonly record struct ApiError(HttpStatusCode StatusCode, string[] Errors);
