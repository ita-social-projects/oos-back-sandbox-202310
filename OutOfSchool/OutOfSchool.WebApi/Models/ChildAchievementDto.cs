﻿namespace OutOfSchool.WebApi.Models;

public class ChildAchievementDto
{
    public string Type { get; set; }

    public DateTime Date { get; set; }

    public string Trainer { get; set; }

    public Guid ChildId { get; set; }

    public Guid WorkshopId { get; set; }
}
