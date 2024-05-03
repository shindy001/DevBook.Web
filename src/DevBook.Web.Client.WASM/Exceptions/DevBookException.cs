namespace DevBook.Web.Client.WASM.Exceptions;

public class DevBookException : Exception
{
	private readonly static string DefaultError = "Sorry, there was an error.";

	public IList<string> Errors { get; } = [];

	public DevBookException()
		: base(DefaultError)
	{
	}

	public DevBookException(string message)
		: base(message)
	{
	}

	public DevBookException(string[] errors)
		: base(DefaultError)
	{
		Errors = errors;
	}

	public DevBookException(string message, string[] errors)
		: base(message)
	{
		Errors = errors;
	}
}
