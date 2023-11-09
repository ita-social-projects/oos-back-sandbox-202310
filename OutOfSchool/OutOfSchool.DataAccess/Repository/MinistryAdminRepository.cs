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

    public async Task<IEnumerable<MinistryAdmin>> GetForMinistryId(int id)
    {
        return await dbContext.MinistryAdmins.Where(x => x.MinistryId == id && x.IsDeleted == false)
            .ToListAsync();
    }

    public async Task DeleteById(Guid id)
    {
        var min = await db.MinistryAdmins.FindAsync(id);
        if (min != null)
        {
            db.Entry(min).State = EntityState.Deleted;
        }

        await dbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public virtual async Task Detach(MinistryAdmin entity)
    {
        db.Entry(entity).State = EntityState.Detached;

        await dbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}
