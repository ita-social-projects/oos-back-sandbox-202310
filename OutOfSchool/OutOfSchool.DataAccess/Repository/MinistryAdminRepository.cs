using Microsoft.EntityFrameworkCore;
using OutOfSchool.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfSchool.Services.Repository;
public class MinistryAdminRepository : EntityRepositorySoftDeleted<Guid, MinistryAdmin>, IMinistryAdminRepository
{
    private readonly OutOfSchoolDbContext db;

    public MinistryAdminRepository(OutOfSchoolDbContext dbContext)
        : base(dbContext)
    {
        db = dbContext;
    }

    public override Task<MinistryAdmin> Create(MinistryAdmin ministryAdmin)
    {
        return base.Create(ministryAdmin);
    }

    public async Task<IEnumerable<ChildAchievement>> GetForMinistryId(int id)
    {
        return (IEnumerable<ChildAchievement>)await dbContext.MinistryAdmins.Where(x => x.MinistryId == id)
            .ToListAsync();
    }

    public async Task DeleteById(Guid id)
    {
        var min = await db.MinistryAdmins.FindAsync(id);
        if (min != null)
        {
            dbContext.MinistryAdmins.Remove(min);
        }

        await dbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}
