using OutOfSchool.WebApi.Models.ChildAchievement;

namespace OutOfSchool.WebApi.Services;

public interface IChildAchievementService
{
    Task<ChildAchievementCreationResponseDto> CreateAchievement(ChildAchievementCreationRequestDto childAchievementCreationRequestDto, string userId);

    Task DeleteAchievement(Guid id, string userId);

    Task<ChildAchievementUpdatingResponseDto> UpdateAchievement(ChildAchievementUpdatingRequestDto childAchievementUpdatingRequestDto, string userId);

    Task<IEnumerable<ChildAchievementGettingDto>> GetAchievementForChildId(Guid id);

    Task<IEnumerable<ChildAchievementGettingDto>> GetAchievementForWorkshopId(Guid id);

    Task<IEnumerable<ChildAchievementGettingDto>> GetAchievementForWorkshopIdChildId(Guid childId, Guid workshopId);

    Task<IEnumerable<ChildAchievementGettingDto>> GetAll();
}
