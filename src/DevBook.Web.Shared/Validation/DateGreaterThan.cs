using System.ComponentModel.DataAnnotations;

namespace DevBook.Web.Shared.Validation;
public class TimeSpanGreaterThanAttribute : ValidationAttribute
{
	private readonly string _comparisonProperty;

	public TimeSpanGreaterThanAttribute(string comparisonProperty)
	{
		_comparisonProperty = comparisonProperty;
	}

	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		if (value is null)
		{
			return ValidationResult.Success;
		}

		var currentTimeSpan = value as TimeSpan? ?? throw new InvalidOperationException("Annotated property is not a DateTime");
		var comparisonValue = validationContext?.ObjectType?.GetProperty(_comparisonProperty)?.GetValue(validationContext.ObjectInstance) as TimeSpan?;

		if (comparisonValue is null || currentTimeSpan < comparisonValue)
		{
			return new ValidationResult(ErrorMessage = "End time must be later than start time");
		}

		return ValidationResult.Success;
	}
}
