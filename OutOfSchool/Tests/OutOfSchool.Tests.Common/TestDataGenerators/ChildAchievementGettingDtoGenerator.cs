using Bogus;
using OutOfSchool.WebApi.Models.ChildAchievement;
using System;
using System.Collections.Generic;

namespace OutOfSchool.Tests.Common.TestDataGenerators;
public class ChildAchievementGettingDtoGenerator
{
    private static readonly Faker<ChildAchievementGettingDto> faker = new Faker<ChildAchievementGettingDto>()
        .RuleFor(x => x.Id, _ => Guid.NewGuid())
        .RuleFor(x => x.Type, f => f.Person.FirstName)
        .RuleFor(x => x.Date, f => f.Person.DateOfBirth)
        .RuleFor(x => x.Name, f => f.Person.FirstName)
        .RuleFor(x => x.Trainer, f => f.Person.FirstName)
        .RuleFor(x => x.TrainerId, _ => Guid.NewGuid())
        .RuleFor(x => x.ChildId, _ => Guid.NewGuid());

    public static ChildAchievementGettingDto Generate() => faker.Generate();

    public static List<ChildAchievementGettingDto> Generate(int count) => faker.Generate(count);
}
