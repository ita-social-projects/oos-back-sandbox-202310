using OutOfSchool.WebApi.Common;
using OutOfSchool.WebApi.Models.ChildAchievement;

namespace OutOfSchool.WebApi.Services;

public interface IChildAchievementTypeService
{
    Task<Result<ChildAchievementType>> GetAchievementTypeById(int id);

    Task<Result<ChildAchievementType>> CreateAchievementType(ChildAchievementTypeRequestDto childAchievementTypeRequestDto);

    Task<Result<object>> DeleteAchievementType(int id);

    Task<Result<IEnumerable<ChildAchievementType>>> GetAllAchievementTypes();
}
