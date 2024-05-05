namespace DevBook.Web.Client.WASM.Errors;

internal sealed record DevBookError(string? Name = "Error", string? Description = "Sorry, there was an error.");
