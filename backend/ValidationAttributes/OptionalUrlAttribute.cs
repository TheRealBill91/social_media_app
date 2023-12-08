using System.ComponentModel.DataAnnotations;

public class OptionalUrlAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            // Return success for null or empty strings, since the URL is optional.
            return ValidationResult.Success;
        }

        // Use the standard URL attribute for validation when there is a value
        var urlAttribute = new UrlAttribute();

        if (!urlAttribute.IsValid(value))
        {
            return new ValidationResult(this.ErrorMessage ?? "Invalid URL format.");
        
        
        
        
        }   

        return ValidationResult.Success;
    }
}
