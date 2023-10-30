using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OutOfSchool.Services.Models;

namespace OutOfSchool.Services.Repository;
public class ChildAchievementTypeRepository : IChildAchievementTypeRepository
{
    private readonly OutOfSchoolDbContext dbContext;

    public ChildAchievementTypeRepository(OutOfSchoolDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<ChildAchievementType> Create(ChildAchievementType childAchievementType)
    {
        await dbContext.ChildAchievementTypes.AddAsync(childAchievementType).ConfigureAwait(false);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        return await Task.FromResult(childAchievementType).ConfigureAwait(false);
    }

    public async Task Delete(int id)
    {
        var achiT = await dbContext.ChildAchievementTypes.FindAsync(id);
        if (achiT != null)
        {
            dbContext.ChildAchievementTypes.Remove(achiT);
        }

        await dbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<ChildAchievementType> GetById(int id)
    {
        return await dbContext.ChildAchievementTypes.FindAsync(id);
    }

    public async Task<IEnumerable<ChildAchievementType>> GetAll()
    {
        return await Task.FromResult(await dbContext.ChildAchievementTypes.ToListAsync()).ConfigureAwait(false);
    }
}
