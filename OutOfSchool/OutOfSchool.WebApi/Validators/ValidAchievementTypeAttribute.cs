using System.ComponentModel.DataAnnotations;

namespace OutOfSchool.WebApi.Validators;

public class ValidAchievementTypeAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string[] validTypes =
            {
            "Переможці міжнародних та всеукраїнських спортивних змагань (індивідуальних та командних)",
            "Призери та учасники міжнародних, всеукраїнських та призери регіональних конкурсів і виставок наукових, технічних, дослідницьких, інноваційних, ІТ проектів",
            "Реципієнти міжнародних грантів",
            "Призери міжнародних культурних конкурсів та фестивалів",
            "Соціально активні категорії учнів",
            "Цифрові інструменти Google для закладів вищої та фахової передвищої освіти",
            "Переможці та учасники олімпіад міжнародного та всеукраїнського рівнів"
            };

        string typeValue = value as string;
        if (validTypes.Contains(typeValue))
        {
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult("Invalid achievement type");
        }
    }
}
