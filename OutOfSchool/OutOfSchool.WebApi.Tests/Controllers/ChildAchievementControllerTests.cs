using Bogus.DataSets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Nest;
using NUnit.Framework;
using OutOfSchool.Common;
using OutOfSchool.Tests.Common.TestDataGenerators;
using OutOfSchool.WebApi.Controllers.V1;
using OutOfSchool.WebApi.Models;
using OutOfSchool.WebApi.Models.ChildAchievement;
using OutOfSchool.WebApi.Models.SocialGroup;
using OutOfSchool.WebApi.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OutOfSchool.WebApi.Tests.Controllers;

[TestFixture]
public class ChildAchievementControllerTests
{
    private ChildAchievementController controller;
    private Mock<IChildAchievementService> service;
    private string currentUserId;
    private Guid childId;
    private List<ChildAchievementGettingDto> childrenAchiGet;

    [SetUp]
    public void Setup()
    {
        service = new Mock<IChildAchievementService>();
        controller = new ChildAchievementController(service.Object);

        childId = Guid.NewGuid();

        currentUserId = Guid.NewGuid().ToString();
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new Claim(IdentityResourceClaimsTypes.Sub, currentUserId) }, "sub"));
        controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        childrenAchiGet =
            ChildAchievementGettingDtoGenerator.Generate(2);
    }

    [Test]
    public void ChildAchievementController_WhenServicesIsNull_ThrowsArgumentNullException()
    {
        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => new ChildAchievementController(null));
    }

    [Test]
    public void GetAllAchievementsForChild_WhenThereNoChild_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(async () => await controller.GetForChildId(Guid.NewGuid()).ConfigureAwait(false));
    }

    [Test]
    public async Task GetChildrenAchievementByChildrenId_WhenThereAreChildrenAchievement_ShouldReturnOkResultObject()
    {
        // Arrange
        service.Setup(x => x.GetAchievementForChildId(childId)).ReturnsAsync(childrenAchiGet);

        // Act
        var result = await controller.GetForChildId(childId).ConfigureAwait(false);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public async Task GetChildrenAchievementByChildrenId_WhenThereIsNoChildrenAchievemnts_ShouldReturnNoContentObjectResult()
    {
        // Arrange
        service.Setup(x => x.GetAchievementForChildId(childId)).ReturnsAsync(Enumerable.Empty<ChildAchievementGettingDto>());

        // Act
        var result = await controller.GetForChildId(childId).ConfigureAwait(false);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }
}
