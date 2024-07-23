using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace DotnetHomework.Api.Validators
{
    public class MaxTagLengthAttribute : ValidationAttribute
    {
        public int MaxLength { get; }

        public MaxTagLengthAttribute(int maxLength)
        {
            this.MaxLength = maxLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; // Allow null values
            }

            if (value is List<string> stringList)
            {
                foreach (var item in stringList)
                {
                    if (item.Length > MaxLength)
                    {
                        var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                        return new ValidationResult(errorMessage);
                    }
                }
                return ValidationResult.Success;
            }

            return new ValidationResult("The field must be a list of strings."); // Handle invalid types
        }

        public override string FormatErrorMessage(string displayName)
        {
            return $"Items in {displayName} cannot exceed {MaxLength} characters.";
        }
    }
}