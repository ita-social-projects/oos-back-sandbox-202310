﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfSchool.Services.Models;
public class ChildAchievement : IKeyedEntity<Guid>
{
    public Guid Id { get; set; }

    public int ChildAchievementTypeId { get; set; }

    public DateTime Date { get; set; }

    public int TrainerId { get; set; }

    public Guid ChildId { get; set; }

    public virtual Child Child { get; set; }

    public Guid WorkshopId { get; set; }

    public virtual Workshop Workshop { get; set; }
}
