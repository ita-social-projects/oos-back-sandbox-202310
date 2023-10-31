using System.ComponentModel.DataAnnotations;

namespace OutOfSchool.WebApi.Models;

public class FavouriteDto
{
    public Guid Id { get; set; }

    [Required]
    public Guid WorkshopId { get; set; }

    [Required]
    public string UserId { get; set; }
}
