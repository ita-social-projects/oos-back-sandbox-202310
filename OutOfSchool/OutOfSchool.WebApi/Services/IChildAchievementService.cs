using OutOfSchool.WebApi.Models;

namespace OutOfSchool.WebApi.Services;

public interface IChildAchievementService
{
    Task<ChildAchievementCreationDto> CreateAchievement(ChildAchievementCreationDto childAchievementCreationDto);

    public Task DeleteAchievement(Guid id);

    public Task<ChildAchievementDto> UpdateAchievement(ChildAchievementDto childAchievementDto);

    public Task<IEnumerable<ChildAchievementGettingDto>> GetAchievementForChildId(Guid id);

    public Task<IEnumerable<ChildAchievementGettingDto>> GetAchievementForWorkshopId(Guid id);

    public Task<IEnumerable<ChildAchievementGettingDto>> GetAchievementForWorkshopIdChildId(Guid childId, Guid workshopId);
}
