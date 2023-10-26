using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;
using OutOfSchool.WebApi.Validators;
namespace OutOfSchool.WebApi.Models;

public class ChildAchievementCreationDto
{
    [Required(ErrorMessage = "Id is required")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Type is required")]
    [ValidAchievementType(ErrorMessage = "Invalid achievement type")]
    public string Type { get; set; }

    [Required(ErrorMessage = "Date is required")]
    [DataType(DataType.Date)]
    [NotFutureAchievementDate(ErrorMessage = "NotFutureAchievementDateAttribute")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Trainer is required")]
    public string Trainer { get; set; }

    [Required(ErrorMessage = "Child id is required")]
    public Guid ChildId { get; set; }

    [Required(ErrorMessage = "Workshop id is required")]
    public Guid WorkshopId { get; set; }
}
