﻿using System;

namespace OutOfSchool.Services.Models;
public class ChildAchievement : IKeyedEntity<Guid>
{
    public Guid Id { get; set; }

    public int ChildAchievementTypeId { get; set; }

    public DateTime Date { get; set; }

    public string Name { get; set; }

    public Guid TrainerId { get; set; }

    public Guid ChildId { get; set; }

    public virtual Child Child { get; set; }
}
