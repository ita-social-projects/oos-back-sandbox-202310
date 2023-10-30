using OutOfSchool.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfSchool.Services.Repository;
public interface IChildAchievementTypeRepository
{
    Task<ChildAchievementType> GetById(int id);

    Task<ChildAchievementType> Create(ChildAchievementType childAchievementType);

    Task Delete(int id);

    Task<IEnumerable<ChildAchievementType>> GetAll();
}
