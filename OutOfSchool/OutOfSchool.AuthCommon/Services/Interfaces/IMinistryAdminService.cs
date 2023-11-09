using Microsoft.AspNetCore.Mvc;
using OutOfSchool.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfSchool.AuthCommon.Services.Interfaces;
public interface IMinistryAdminService
{
    Task<ResponseDto> CreateMinistryAdminAsync(
        CreateMinistryAdminDto ministryAdminDto,
        IUrlHelper url,
        string userId);

    Task<ResponseDto> DeleteMinistryAdminAsync(
        Guid ministryAdminId,
        string userId);

    Task<ResponseDto> UpdateMinistryAdminAsync(
        UpdateMinistryAdminDto ministryAdminDto,
        string userId);

    Task<ResponseDto> BlockMinistryAdminAsync(
        Guid ministryAdminId,
        string userId);
}
