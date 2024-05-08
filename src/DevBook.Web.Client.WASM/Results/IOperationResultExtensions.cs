namespace DevBook.Web.Client.WASM.Results;

public static class IOperationResultExtensions
{
	internal static DevBookError CreateError(this IOperationResult operationResult)
	{
		var error = operationResult.Errors.FirstOrDefault();
		return new DevBookError(error?.Exception?.GetType().Name ?? "Unknown Error", error?.Message ?? "Sorry, there was an error.");
	}

	internal static OneOf<TResult, DevBookError> Unwrap<TResult>(this IOperationResult operationResult, Func<TResult> unwrapDataFunc)
	{
		if (operationResult.IsSuccessResult())
		{
			try
			{
				return unwrapDataFunc();
			}
			catch (Exception e)
			{
				return new DevBookError(Description: e.Message);
			}
		}

		var error = operationResult.Errors.FirstOrDefault();
		return new DevBookError(error?.Exception?.GetType().Name ?? "Unknown Error", error?.Message ?? "Sorry, there was an error.");
	}

	internal static OneOf<Success, DevBookError> Unwrap(this IOperationResult operationResult)
	{
		if (operationResult.IsSuccessResult())
		{
			return new Success();
		}

		var error = operationResult.Errors.FirstOrDefault();
		return new DevBookError(error?.Exception?.GetType().Name ?? "Unknown Error", error?.Message ?? "Sorry, there was an error.");
	}
}
