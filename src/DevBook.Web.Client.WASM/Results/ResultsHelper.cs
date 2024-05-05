using DevBook.Web.Client.WASM.Errors;
using StrawberryShake;

namespace DevBook.Web.Client.WASM.Results;

internal static class ResultsHelper
{
	internal static OneOf<Success, DevBookError> FromOperationResult(IOperationResult operationResult)
	{
		if (operationResult.IsSuccessResult())
		{
			return new Success();
		}

		var error = operationResult.Errors.FirstOrDefault();
		return new DevBookError(error?.Exception?.GetType().Name ?? "Unknown Error", error?.Message ?? "Sorry, there was an error.");
	}
}
