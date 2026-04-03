using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Shared.Validation
{
    public class LessThanOrEqualToOtherPropertyAttribute : ValidationAttribute
    {
        private readonly string _otherPropertyName;

        public LessThanOrEqualToOtherPropertyAttribute(string otherPropertyName, string errorMessage) : base(errorMessage)
        {
            _otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherPropertyName);

            if (otherPropertyInfo == null)
            {
                return new ValidationResult($"Unknown property {_otherPropertyName}");
            }

            var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

            // Basic check to ensure values are comparable (e.g., both are integers)
            if (value != null && otherPropertyValue != null && value is IComparable comparableValue && otherPropertyValue is IComparable comparableOtherValue)
            {
                if (comparableValue.CompareTo(comparableOtherValue) > 0)
                {
                    return new ValidationResult(ErrorMessageString);
                }
            }
            else if (value == null)
            {
                return ValidationResult.Success; // Or enforce Required validation separately
            }

            return ValidationResult.Success;
        }
    }
}
