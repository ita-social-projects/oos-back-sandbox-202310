namespace OutOfSchool.WebApi.Services;

public interface IChildAchievementTypeService
{
    Task<ChildAchievementType> GetById(int id);

    Task<ChildAchievementType> Create(ChildAchievementType childAchievementType);

    Task Delete(int id);

    Task<ChildAchievementType> Update(ChildAchievementType childAchievementType);

    Task<IEnumerable<ChildAchievementType>> GetAll();
}
