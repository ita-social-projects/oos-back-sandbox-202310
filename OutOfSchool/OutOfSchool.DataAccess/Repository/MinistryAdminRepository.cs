using Microsoft.EntityFrameworkCore;
using OutOfSchool.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfSchool.Services.Repository;
public class MinistryAdminRepository : EntityRepositorySoftDeleted<Guid, MinistryAdmin>, IEntityRepository<Guid, MinistryAdmin>
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

    public async Task<IEnumerable<ChildAchievement>> GetForMinistryId(Guid id)
    {
        return await dbContext.MinistryAdmins.Where(x => x.MinistryId == id)
            .ToListAsync();
    }
}
