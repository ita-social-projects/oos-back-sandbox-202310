using System.ComponentModel.DataAnnotations;
using OutOfSchool.WebApi.Validators;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace OutOfSchool.WebApi.Models.ChildAchievement;

public class ChildAchievementUpdatingResponseDto
{
    [Required(ErrorMessage = "Id is required")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Type is required")]
    public int ChildAchievementTypeId { get; set; }

    [Required(ErrorMessage = "Date is required")]
    [DataType(DataType.Date)]
    [NotFutureAchievementDate(ErrorMessage = "NotFutureAchievementDateAttribute")]
    public DateTime Date { get; set; }

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
