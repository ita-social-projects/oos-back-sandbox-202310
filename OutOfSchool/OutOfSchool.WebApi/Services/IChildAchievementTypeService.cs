using OutOfSchool.WebApi.Models.ChildAchievement;

namespace OutOfSchool.WebApi.Services;

public interface IChildAchievementTypeService
{
    Task<ChildAchievementType> GetAchievementTypeById(int id);

    Task<ChildAchievementType> CreateAchievementType(ChildAchievementTypeRequestDto childAchievementTypeRequestDto);

    Task DeleteAchievementType(int id);

    Task<IEnumerable<ChildAchievementType>> GetAllAchievementTypes();
}
