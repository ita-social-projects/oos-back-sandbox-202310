using OutOfSchool.WebApi.Common;
using OutOfSchool.WebApi.Models.ChildAchievement;

namespace OutOfSchool.WebApi.Services;

public interface IChildAchievementService
{
    Task<Result<ChildAchievementCreationResponseDto>> CreateAchievement(ChildAchievementCreationRequestDto childAchievementCreationRequestDto, string userId);

    Task<Result<object>> DeleteAchievement(Guid id, string userId);

    Task<Result<ChildAchievementUpdatingResponseDto>> UpdateAchievement(ChildAchievementUpdatingRequestDto childAchievementUpdatingRequestDto, string userId);

    Task<Result<IEnumerable<ChildAchievementGettingDto>>> GetAchievementForChildId(Guid id);

    Task<Result<IEnumerable<ChildAchievementGettingDto>>> GetAchievementForWorkshopId(Guid id);

    Task<Result<IEnumerable<ChildAchievementGettingDto>>> GetAchievementForWorkshopIdChildId(Guid childId, Guid workshopId);

    Task<Result<IEnumerable<ChildAchievementGettingDto>>> GetAll();
}
