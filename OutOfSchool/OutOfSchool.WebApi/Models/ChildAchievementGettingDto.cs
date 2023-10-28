using OutOfSchool.WebApi.Validators;
using System.ComponentModel.DataAnnotations;

namespace OutOfSchool.WebApi.Models;

public class ChildAchievementGettingDto
{
    [Required(ErrorMessage = "Type is required")]
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
