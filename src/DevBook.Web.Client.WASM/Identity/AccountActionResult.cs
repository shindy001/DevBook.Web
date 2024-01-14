namespace DevBook.Web.Client.WASM.Identity;

/// <summary>
/// Response for login and registration.
/// </summary>
public sealed record AccountActionResult
{
	/// <summary>
	/// Gets or sets a value indicating whether the action was successful.
	/// </summary>
	public bool Succeeded { get; set; }

	/// <summary>
	/// On failure, the problem details are parsed and returned in this array.
	/// </summary>
	public string[] ErrorList { get; set; } = [];
}
