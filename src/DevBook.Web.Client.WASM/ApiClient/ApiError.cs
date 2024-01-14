using System.Net;

namespace DevBook.Web.Client.WASM.ApiClient;

internal readonly record struct ApiError(HttpStatusCode StatusCode, string[] Errors);
