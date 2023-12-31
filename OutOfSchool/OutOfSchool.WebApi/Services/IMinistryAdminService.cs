﻿using Microsoft.AspNetCore.Mvc;
using OutOfSchool.Common.Models;
using OutOfSchool.WebApi.Common;
using OutOfSchool.WebApi.Models.Ministry;

namespace OutOfSchool.WebApi.Services;

public interface IMinistryAdminService
{
    Task<Either<ErrorResponse, CreateMinistryAdminDto>> CreateMinistryAdminAsync(
        string userId,
        CreateMinistryAdminDto ministryAdminDto,
        string token);

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
        string token,
        bool isBlocked);

    Task<Result<MinistryAdminGettingDto>> GetById(Guid id);

    Task<Result<IEnumerable<MinistryAdminGettingDto>>> GetAll();

    Task<Result<IEnumerable<MinistryAdminGettingDto>>> GetForMinistryId(int id);
}
