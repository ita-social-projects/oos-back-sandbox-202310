using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfSchool.Common.Models;
public class UpdateMinistryAdminDto : AdminBaseDto
{
    [Required(ErrorMessage = "Id is required")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Settlement is required")]
    public int SettlementId { get; set; }

    [Required(ErrorMessage = "MinistryId is required")]
    public int MinistryId { get; set; }
}
