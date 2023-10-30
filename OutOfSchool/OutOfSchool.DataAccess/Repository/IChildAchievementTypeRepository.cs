using System.Collections.Generic;
using System.Threading.Tasks;
using OutOfSchool.Services.Models;

namespace OutOfSchool.Services.Repository;
public interface IChildAchievementTypeRepository
{
    Task<ChildAchievementType> GetById(int id);

    Task<ChildAchievementType> Create(ChildAchievementType childAchievementType);

    Task Delete(int id);

    Task<IEnumerable<ChildAchievementType>> GetAll();
}
