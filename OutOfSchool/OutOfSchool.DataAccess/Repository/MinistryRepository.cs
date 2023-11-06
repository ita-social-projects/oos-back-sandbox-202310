using System.Threading.Tasks;
using OutOfSchool.Services.Models;

namespace OutOfSchool.Services.Repository;
public class MinistryRepository : EntityRepository<int, Ministry>, IEntityRepository<int, Ministry>
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
}
