using Microsoft.Extensions.Localization;
using OutOfSchool.WebApi.Enums;
using System.ComponentModel.DataAnnotations;

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
