using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OutOfSchool.Services.Models;

namespace OutOfSchool.Services.Repository;
public interface IChildAchievementRepository
{
    Task<ChildAchievement> Create(ChildAchievement childAchievement);

    Task<ChildAchievement> Update(ChildAchievement childAchievement);

    Task Delete(Guid id);

    Task<ChildAchievement> GetById(Guid id);

    Task<IEnumerable<ChildAchievement>> GetForWorkshop(Guid id);

    Task<IEnumerable<ChildAchievement>> GetForChild(Guid id);

    Task<IEnumerable<ChildAchievement>> GetForWorkshopChild(Guid childId, Guid workshopId);

    Task<IEnumerable<ChildAchievement>> GetAll();
}
