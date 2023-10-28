using Microsoft.EntityFrameworkCore;
using OutOfSchool.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfSchool.Services.Repository;
public class ChildAchievementTypeRepository : IChildAchievementTypeRepository
{
    private readonly OutOfSchoolDbContext dbContext;

    public ChildAchievementTypeRepository(OutOfSchoolDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<ChildAchievementType> GetById(int id)
    {
        return await dbContext.ChildAchievementTypes.FindAsync(id);
    }
}
