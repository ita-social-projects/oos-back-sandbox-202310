using Microsoft.AspNetCore.Mvc;
using OutOfSchool.Common.Models;

namespace OutOfSchool.WebApi.Services.MinistryAdminOperations;

public interface IMinistryAdminOperationsService
{
    public Task<Either<ErrorResponse, CreateMinistryAdminDto>> CreateMinistryAdminAsync(string userId, CreateMinistryAdminDto ministryAdminDto, string token);

    Task<Either<ErrorResponse, ActionResult>> DeleteMinistryAdminAsync(
        Guid ministryAdminId,
        string userId,
        string token);

    Task<Either<ErrorResponse, UpdateMinistryAdminDto>> UpdateMinistryAdminAsync(
        UpdateMinistryAdminDto updateMinistryAdminDto,
        string userId,
        string token);

    Task<Either<ErrorResponse, ActionResult>> BlockMinistryAdminAsync(
        Guid ministryAdminId,
        string userId,
        string token);
}
