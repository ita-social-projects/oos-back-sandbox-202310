using OutOfSchool.Common.Models;

namespace OutOfSchool.WebApi.Services.MinistryAdminOperations;

public interface IMinistryAdminOperationsService
{
    public Task<Either<ErrorResponse, CreateMinistryAdminDto>> CreateMinistryAdminAsync(string userId, CreateMinistryAdminDto ministryAdminDto, string token);
}
