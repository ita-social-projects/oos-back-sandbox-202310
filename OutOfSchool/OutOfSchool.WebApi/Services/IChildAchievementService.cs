using OutOfSchool.WebApi.Models.ChildAchievement;

namespace OutOfSchool.WebApi.Services;

public interface IChildAchievementService
{
    Task<ChildAchievementCreationDto> CreateAchievement(ChildAchievementCreationRequestDto childAchievementCreationRequestDto, string userId);

    public Task DeleteAchievement(Guid id, string userId);

    public Task<ChildAchievementUpdatingDto> UpdateAchievement(ChildAchievementUpdatingDto childAchievementDto, string userId);

    public Task<IEnumerable<ChildAchievementGettingDto>> GetAchievementForChildId(Guid id);

    public Task<IEnumerable<ChildAchievementGettingDto>> GetAchievementForWorkshopId(Guid id);

    public Task<IEnumerable<ChildAchievementGettingDto>> GetAchievementForWorkshopIdChildId(Guid childId, Guid workshopId);

    Task<IEnumerable<ChildAchievementGettingDto>> GetAll();
}
