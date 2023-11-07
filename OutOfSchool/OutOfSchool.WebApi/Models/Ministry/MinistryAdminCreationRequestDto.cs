using OutOfSchool.Services.Enums;
using System.ComponentModel.DataAnnotations;

namespace OutOfSchool.WebApi.Models.Ministry;

public class MinistryAdminCreationRequestDto
{
    [Required(ErrorMessage = "FirstName is required")]
    [StringLength(50, ErrorMessage = "FirstName cannot exceed 50 characters")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "MiddleName is required")]
    [StringLength(50, ErrorMessage = "MiddleName cannot exceed 50 characters")]
    public string MiddleName { get; set; }

    [Required(ErrorMessage = "LastName is required")]
    [StringLength(50, ErrorMessage = "LastName cannot exceed 50 characters")]
    public string LastName { get; set; }

    [RegularExpression(@"^\+380\d{9}$", ErrorMessage = "Non valid Ukrainian phone number.")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Settlement is required")]
    public int SettlementId { get; set; }

    [EmailAddress(ErrorMessage = "Non valid email address.")]
    public string EmailAddress { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 50 characters")]
    public string Password { get; set; }

    [Required(ErrorMessage = "MinistryId is required")]
    public int MinistryId { get; set; }
}
