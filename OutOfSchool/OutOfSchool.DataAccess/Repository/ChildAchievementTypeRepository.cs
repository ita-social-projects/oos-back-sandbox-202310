using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OutOfSchool.Services.Models;

namespace OutOfSchool.Services.Repository;
public class ChildAchievementTypeRepository : EntityRepositoryBase<int, ChildAchievementType>, IChildAchievementTypeRepository
{
    private readonly OutOfSchoolDbContext dbContext;

    public ChildAchievementTypeRepository(OutOfSchoolDbContext dbContext)
        : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task DeleteById(int id)
    {
        var achiT = await dbContext.ChildAchievementTypes.FindAsync(id);
        if (achiT != null)
        {
            dbContext.ChildAchievementTypes.Remove(achiT);
        }

        await dbContext.SaveChangesAsync().ConfigureAwait(false);
    }

}
