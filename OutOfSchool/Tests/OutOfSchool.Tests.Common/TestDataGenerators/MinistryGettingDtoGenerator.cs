using Bogus;
using OutOfSchool.Services.Models;
using OutOfSchool.WebApi.Models.ChildAchievement;
using OutOfSchool.WebApi.Models.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfSchool.Tests.Common.TestDataGenerators;
public class MinistryGettingDtoGenerator
{
    private static readonly Faker<MinistryGettingDto> faker = new Faker<MinistryGettingDto>()
        .RuleFor(x => x.Id, f => f.Random.Int())
        .RuleFor(x => x.Name, f => f.Person.FirstName);

    public static MinistryGettingDto Generate() => faker.Generate();

    public static List<MinistryGettingDto> Generate(int count) => faker.Generate(count);
}
