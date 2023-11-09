using System;
using System.ComponentModel.DataAnnotations;

namespace OutOfSchool.Common.Models;
public class CreateMinistryAdminDto: AdminBaseDto
{
    [DataType(DataType.DateTime)]
    public DateTimeOffset CreatingTime { get; set; }

    public string ReturnUrl { get; set; }

    [Required(ErrorMessage = "Settlement is required")]
    public int SettlementId { get; set; }

    [Required(ErrorMessage = "MinistryId is required")]
    public int MinistryId { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    public string UserId { get; set; }
}
