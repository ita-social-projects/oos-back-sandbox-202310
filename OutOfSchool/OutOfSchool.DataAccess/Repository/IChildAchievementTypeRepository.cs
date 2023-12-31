﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OutOfSchool.Services.Models;

namespace OutOfSchool.Services.Repository;
public interface IChildAchievementTypeRepository : IEntityRepository<int, ChildAchievementType>
{
    Task DeleteById(int id);
}
