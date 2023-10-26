using Microsoft.EntityFrameworkCore;
using OutOfSchool.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfSchool.Services.Repository;
public class ChildAchievementRepository : IChildAchievementRepository
{
    protected readonly OutOfSchoolDbContext dbContext;
    public ChildAchievementRepository(OutOfSchoolDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<ChildAchievement> Create(ChildAchievement childAchievement)
    {
        await dbContext.ChildAchievements.AddAsync(childAchievement).ConfigureAwait(false);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        return await Task.FromResult(childAchievement).ConfigureAwait(false);
    }

    public async Task Delete(Guid id)
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

    public async Task<ChildAchievement> Update(ChildAchievement childAchievement)
    {
        dbContext.Entry(childAchievement).State = EntityState.Modified;
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        return await Task.FromResult(childAchievement).ConfigureAwait(false);
    }
}
