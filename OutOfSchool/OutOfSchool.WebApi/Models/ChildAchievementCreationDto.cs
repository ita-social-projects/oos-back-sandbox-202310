using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;
using OutOfSchool.WebApi.Validators;
namespace OutOfSchool.WebApi.Models;

public class ChildAchievementCreationDto
{
    [Required(ErrorMessage = "Type is required")]
    public int ChildAchievementTypeId { get; set; }

    [Required(ErrorMessage = "Trainer is required")]
    public string Trainer { get; set; }

    [Required(ErrorMessage = "Child id is required")]
    public Guid ChildId { get; set; }

    [Required(ErrorMessage = "Workshop id is required")]
    public Guid WorkshopId { get; set; }
}
