using System.ComponentModel.DataAnnotations;

namespace OutOfSchool.WebApi.Models.ChildAchievement;

public class ChildAchievementCreationRequestDto
{
    [Required(ErrorMessage = "Type is required")]
    public int ChildAchievementTypeId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(2000, ErrorMessage = "Name cannot exceed 2000 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Trainer is required")]
    public Guid TrainerId { get; set; }

    [Required(ErrorMessage = "Application id is required")]
    public Guid ApplicationId { get; set; }
}
