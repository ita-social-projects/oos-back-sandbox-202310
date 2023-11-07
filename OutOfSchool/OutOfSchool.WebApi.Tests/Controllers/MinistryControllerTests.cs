using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OutOfSchool.Common;
using OutOfSchool.Services.Models;
using OutOfSchool.Tests.Common.TestDataGenerators;
using OutOfSchool.WebApi.Common;
using OutOfSchool.WebApi.Controllers.V1;
using OutOfSchool.WebApi.Models.ChildAchievement;
using OutOfSchool.WebApi.Models.Ministry;
using OutOfSchool.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OutOfSchool.WebApi.Tests.Controllers;

[TestFixture]
public class MinistryControllerTests
{
    private MinistryController controller;
    private Mock<IMinistryService> service;
    private List<MinistryGettingDto> ministryGet;

    [SetUp]
    public void Setup()
    {
        service = new Mock<IMinistryService>();
        controller = new MinistryController(service.Object);

        ministryGet =
            MinistryGettingDtoGenerator.Generate(2);
    }

    [Test]
    public void MinistryController_WhenServicesIsNull_ThrowsArgumentNullException()
    {
        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => new MinistryController(null));
    }

    [Test]
    public async Task GetAllMinistry_WhenThereAreMinistries_ShouldReturnOkResultObject()
    {
        // Arrange
        service.Setup(x => x.GetAll()).ReturnsAsync(Result<IEnumerable<MinistryGettingDto>>.Success(ministryGet));

        // Act
        var result = await controller.GetAll().ConfigureAwait(false);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public async Task GetAllMinistry_WhenThereIsNoMinistries_ShouldReturnNoContentObjectResult()
    {
        // Arrange
        service.Setup(x => x.GetAll()).ReturnsAsync(Result<IEnumerable<MinistryGettingDto>>.Success(Enumerable.Empty<MinistryGettingDto>()));

        // Act
        var result = await controller.GetAll().ConfigureAwait(false);

        // Assert
        Assert.IsInstanceOf<NoContentResult>(result);
    }
}
