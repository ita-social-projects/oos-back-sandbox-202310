using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OutOfSchool.Services.Models;

namespace OutOfSchool.Services.Repository;
public class ChildAchievementRepository : EntityRepositoryBase<Guid, ChildAchievement>, IChildAchievementRepository
{
    protected readonly OutOfSchoolDbContext dbContext;

    public ChildAchievementRepository(OutOfSchoolDbContext dbContext)
        : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task DeleteById(Guid id)
    {
        var achi = await dbContext.ChildAchievements.FindAsync(id);
        if (achi != null)
        {
            dbContext.ChildAchievements.Remove(achi);
        }

        await dbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<ChildAchievement>> GetForChild(Guid id)
    {
        return await dbContext.ChildAchievements.Where(x => x.ChildId == id)
            .ToListAsync();
    }

    public async Task<IEnumerable<ChildAchievement>> GetForWorkshop(Guid id)
    {
        return await dbContext.ChildAchievements.Where(x => x.WorkshopId == id)
            .ToListAsync();
    }

    public async Task<IEnumerable<ChildAchievement>> GetForWorkshopChild(Guid childId, Guid workshopId)
    {
        return await dbContext.ChildAchievements
            .Where(x => x.ChildId == childId && x.WorkshopId == workshopId)
            .ToListAsync();
    }
}
