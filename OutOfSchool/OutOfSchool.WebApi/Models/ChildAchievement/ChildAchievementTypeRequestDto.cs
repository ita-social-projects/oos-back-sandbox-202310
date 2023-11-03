using System.ComponentModel.DataAnnotations;
using OutOfSchool.WebApi.Validators;

namespace OutOfSchool.WebApi.Models.ChildAchievement;

public class ChildAchievementTypeRequestDto
{
    [Required(ErrorMessage = "Type is required")]
    public string Type { get; set; }

    [Required(ErrorMessage = "Localization is required")]
    [AchievementTypeLocalization(ErrorMessage = "Invalid localization.")]
    public string Localization { get; set; }
}
