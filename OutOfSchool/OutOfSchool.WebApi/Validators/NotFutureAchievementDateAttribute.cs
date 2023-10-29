using System.ComponentModel.DataAnnotations;

namespace OutOfSchool.WebApi.Validators;

public class NotFutureAchievementDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var date = (DateTime)value;

        if (date > DateTime.Today)
        {
            return new ValidationResult("Date should be in the future");
        }

        return ValidationResult.Success;
    }
}
