using OutOfSchool.WebApi.Validators;
using System.ComponentModel.DataAnnotations;

namespace OutOfSchool.WebApi.Models.ChildAchievement;

public class ChildAchievementTypeRequestDto
{
    [Required(ErrorMessage = "Type is required")]
    public string Type { get; set; }

    [Required(ErrorMessage = "Localization is required")]
    [AchievementTypeLocalization(ErrorMessage = "Invalid localization.")]
    public string Localization { get; set; }
}
