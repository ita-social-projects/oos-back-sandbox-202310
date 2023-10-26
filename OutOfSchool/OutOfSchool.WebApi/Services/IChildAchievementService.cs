using OutOfSchool.WebApi.Models;

namespace OutOfSchool.WebApi.Services;

public interface IChildAchievementService
{
    Task<ChildAchievementDto> CreateAchievement(ChildAchievementDto childAchievementDto);

    public Task DeleteAchievement(ChildAchievementDto childAchievementDto);

    public Task<ChildAchievementDto> UpdateAchievement(ChildAchievementDto childAchievementDto);

    public Task<IEnumerable<ChildAchievementDto>> GetAchievementForChildId(Guid id);

    public Task<IEnumerable<ChildAchievementDto>> GetAchievementForWorkshopId(Guid id);

    public Task<IEnumerable<ChildAchievementDto>> GetAchievementForWorkshopIdChildId(Guid childId, Guid workshopId);
}
