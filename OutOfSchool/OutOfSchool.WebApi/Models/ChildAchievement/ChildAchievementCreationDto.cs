using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;
using OutOfSchool.WebApi.Validators;

namespace OutOfSchool.WebApi.Models.ChildAchievement;

public class ChildAchievementCreationDto
{
    [Required(ErrorMessage = "Type is required")]
    public int ChildAchievementTypeId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(2000, ErrorMessage = "Name cannot exceed 2000 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Trainer is required")]
    public Guid TrainerId { get; set; }

    [Required(ErrorMessage = "Child id is required")]
    public Guid ChildId { get; set; }

    [Required(ErrorMessage = "Workshop id is required")]
    public Guid WorkshopId { get; set; }
}
