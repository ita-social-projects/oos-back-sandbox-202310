using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OutOfSchool.Services.Models;

namespace OutOfSchool.Services.Repository;
public interface IChildAchievementRepository : IEntityRepository<Guid, ChildAchievement>
{
    Task DeleteById(Guid id);

    Task<IEnumerable<ChildAchievement>> GetForWorkshop(Guid id);

    Task<IEnumerable<ChildAchievement>> GetForChild(Guid id);

    Task<IEnumerable<ChildAchievement>> GetForWorkshopChild(Guid childId, Guid workshopId);
}
