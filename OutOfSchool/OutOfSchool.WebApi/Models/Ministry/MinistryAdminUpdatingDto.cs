using OutOfSchool.Services.Enums;

namespace OutOfSchool.WebApi.Models.Ministry;

public class MinistryAdminUpdatingDto
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public string PhoneNumber { get; set; }

    public string Settlement { get; set; }

    public string EmailAddress { get; set; }

    public string Password { get; set; }

    public int MinistryId { get; set; }
}
