using System;
using OutOfSchool.Services.Enums;

namespace OutOfSchool.Services.Models;
public class MinistryAdmin : IKeyedEntity<Guid>, ISoftDeleted
{
    public Guid Id { get; set; }

    public bool IsDeleted { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public string PhoneNumber { get; set; }

    public int SettlementId { get; set; }

    public string EmailAddress { get; set; }

    public string Password { get; set; }

    public MinistryAdminStatus Status { get; set; }

    public int MinistryId { get; set; }

    public Ministry Ministry { get; set; }
}
