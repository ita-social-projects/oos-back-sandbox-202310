using OutOfSchool.WebApi.Common;
using OutOfSchool.WebApi.Models.ChildAchievement;
using OutOfSchool.WebApi.Models.Ministry;

namespace OutOfSchool.WebApi.Services;

public interface IMinistryService
{
    Task<Result<MinistryCreationResponseDto>> Create(MinistryCreationRequestDto ministryCreationRequestDto);

    Task<Result<object>> Delete(int id);

    Task<Result<MinistryGettingDto>> GetById(int id);

    Task<Result<IEnumerable<MinistryGettingDto>>> GetAll();
}
