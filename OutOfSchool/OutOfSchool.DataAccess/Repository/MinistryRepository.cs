using System;
using System.Threading.Tasks;
using OutOfSchool.Services.Models;

namespace OutOfSchool.Services.Repository;
public class MinistryRepository : EntityRepository<int, Ministry>, IMinistryRepository
{
    private readonly OutOfSchoolDbContext db;

    public MinistryRepository(OutOfSchoolDbContext dbContext)
        : base(dbContext)
    {
        db = dbContext;
    }

    public override Task<Ministry> Create(Ministry ministry)
    {
        return base.Create(ministry);
    }

    public async Task DeleteById(int id)
    {
        var min = await db.Ministries.FindAsync(id);
        if (min != null)
        {
            dbContext.Ministries.Remove(min);
        }

        await dbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}
