using System.ComponentModel.DataAnnotations;

namespace OutOfSchool.WebApi.Models.Ministry;

public class MinistryCreationRequestDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; }
}
