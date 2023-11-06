using OutOfSchool.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfSchool.Services.Repository;
public interface IMinistryAdminRepository : IEntityRepository<Guid, MinistryAdmin>
{
    Task DeleteById(Guid id);

    Task<IEnumerable<MinistryAdmin>> GetForMinistryId(int id);

    Task Approve(Guid id);
}
