﻿using OutOfSchool.WebApi.Common;
using OutOfSchool.WebApi.Models.Ministry;

namespace OutOfSchool.WebApi.Services;

public interface IMinistryAdminService
{
    Task<Result<MinistryAdminCreationResponseDto>> Create(MinistryAdminCreationRequestDto ministryAdminCreationRequestDto);

    Task<Result<object>> Delete(Guid id);

    Task<Result<MinistryAdminUpdatingDto>> Update(MinistryAdminUpdatingDto ministryAdminUpdatingDto);

    Task<Result<MinistryAdminGettingDto>> GetById(Guid id);

    Task<Result<IEnumerable<MinistryAdminGettingDto>>> GetAll();

    Task<Result<IEnumerable<MinistryAdminGettingDto>>> GetForMinistryId(int id);

    Task<Result<object>> Approve(Guid id);
}