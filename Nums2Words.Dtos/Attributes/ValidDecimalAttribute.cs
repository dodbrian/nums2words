using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Nums2Words.Dtos.Attributes;

public class ValidDecimalAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null) return new ValidationResult("Value cannot be null");
        if (value is not string stringValue) return new ValidationResult("Value must be of type string");

        // Number format provided in the requirements matches fr-FR culture info.
        var specificCulture = CultureInfo.CreateSpecificCulture("fr-FR");

        return decimal.TryParse(stringValue, specificCulture, out _)
            ? ValidationResult.Success
            : new ValidationResult("Provided value cannot be parsed as decimal");
    }
}
