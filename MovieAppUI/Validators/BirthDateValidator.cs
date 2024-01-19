using System.ComponentModel.DataAnnotations;

namespace MovieAppUI.Validators
{
    public class BirthDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime birthDate)
            {
                if (birthDate > DateTime.Now)
                {
                    return new ValidationResult("Birth date can not be in the future");
                }
                return ValidationResult.Success!;
            }
            return new ValidationResult("Birth date is not valid.");
        }
    }
}
