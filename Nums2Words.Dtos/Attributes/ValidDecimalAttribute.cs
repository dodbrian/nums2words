using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Nums2Words.Dtos.Attributes;

public class ValidDecimalAttribute : ValidationAttribute
{
    public double Max { get; init; }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null) return new ValidationResult("Value cannot be null");
        if (value is not string stringValue) return new ValidationResult("Value must be of type string");

        // Number format provided in the requirements matches fr-FR culture info.
        var specificCulture = CultureInfo.CreateSpecificCulture("fr-FR");

        if (!decimal.TryParse(stringValue, specificCulture, out var parsedValue))
            return new ValidationResult("Provided value cannot be parsed as decimal");

        return Math.Abs(parsedValue) > (decimal)Max
            ? new ValidationResult($"Provided value cannot be bigger than {Max}")
            : ValidationResult.Success;
    }
}
