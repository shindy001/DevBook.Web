namespace DevBook.Web.Client.WASM.Results;

internal sealed record DevBookError(string? Name = "Error", string? Description = "Sorry, there was an error.");
