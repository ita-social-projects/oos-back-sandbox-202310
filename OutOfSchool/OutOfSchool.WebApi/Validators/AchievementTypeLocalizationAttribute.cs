using System.ComponentModel.DataAnnotations;
using OutOfSchool.WebApi.Enums;

namespace OutOfSchool.WebApi.Validators;

public class AchievementTypeLocalizationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string localization = value as string;

        string[] localizationValues = Enum.GetNames(typeof(LocalizationType));

        foreach (string s in localizationValues)
        {
            if (localization == s)
            {
                return ValidationResult.Success;
            }
        }

        return new ValidationResult("Invalid localization.");
    }
}
