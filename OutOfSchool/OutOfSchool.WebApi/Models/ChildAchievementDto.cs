using Microsoft.Build.Framework;
using OutOfSchool.WebApi.Validators;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace OutOfSchool.WebApi.Models;

public class ChildAchievementDto
{
    [Required(ErrorMessage = "Type is required")]
    public int ChildAchievementTypeId { get; set; }

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
