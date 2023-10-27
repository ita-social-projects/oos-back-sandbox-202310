﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OutOfSchool.Services.Models;

namespace OutOfSchool.Services.Repository;

public interface IApplicationRepository : IEntityRepositorySoftDeleted<Guid, Application>
{
    Task<Application> Update(Application entity, Action<Application> onSaveChanges);

    Task<int> GetCountByWorkshop(Guid workshopId);

    Task<int> UpdateAllApprovedApplications();

    Task<IEnumerable<Application>> GetForWorkshopChild(Guid childId, Guid workshopId);
}