using System;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

using OutOfSchool.Services.Models;
using OutOfSchool.Services.Repository;
using OutOfSchool.WebApi.Models;
using OutOfSchool.WebApi.Services;

namespace OutOfSchool.WebApi.Tests.Services;
public class FavouriteServiceTests
{
    private FavouriteService favouriteService;
    private Mock<IEntityRepository<Guid, Favourite>> favouriteRepositoryMock;
    private Mock<ILogger<FavouriteService>> loggerMock;
    private Mock<IMapper> mapperMock;
    private Mock<IWorkshopService> workshopServiceMock;

    [SetUp]
    public void Setup()
    {
        favouriteRepositoryMock = new Mock<IEntityRepository<Guid, Favourite>>();
        loggerMock = new Mock<ILogger<FavouriteService>>();
        mapperMock = new Mock<IMapper>();
        workshopServiceMock = new Mock<IWorkshopService>();

        favouriteService = new FavouriteService(
            favouriteRepositoryMock.Object,
            loggerMock.Object,
            mapperMock.Object,
            workshopServiceMock.Object);
    }

    [Test]
    public void Create_NullFavouriteDto_ReturnsArgumentNullException()
    {
        Assert.ThrowsAsync<ArgumentNullException>(() => favouriteService.Create(null));
    }

    [Test]
    public async Task Create_ValidFavouriteDto_ReturnsFavouriteDto()
    {
        // Arrange
        var workshopId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();

        var dto = new FavouriteDto
        {
            Id = Guid.NewGuid(),
            WorkshopId = workshopId,
            UserId = userId,
        };

        var resultDto = new Favourite
        {
            WorkshopId = workshopId,
            UserId = userId,
        };

        mapperMock.Setup(mapper => mapper.Map<Favourite>(dto)).Returns(resultDto);
        favouriteRepositoryMock.Setup(repo => repo.Create(resultDto)).ReturnsAsync(resultDto);
        mapperMock.Setup(mapper => mapper.Map<FavouriteDto>(resultDto)).Returns(dto);

        // Act
        var result = await favouriteService.Create(dto);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(dto.WorkshopId, result.WorkshopId);
        Assert.AreEqual(dto.UserId, result.UserId);
    }

    [Test]
    public async Task Delete_ShouldDeleteFavourite()
    {
        // Arrange
        var id = Guid.NewGuid();
        var favourite = new Favourite { Id = id };
        favouriteRepositoryMock.Setup(repo => repo.GetById(id)).ReturnsAsync(favourite);
        favouriteRepositoryMock.Setup(repo => repo.Delete(favourite)).Returns(Task.CompletedTask);

        // Act
        await favouriteService.Delete(id);

        // Assert
        favouriteRepositoryMock.Verify(repo => repo.Delete(favourite), Times.Once);
    }

    [Test]
    public async Task GetById_ShouldReturnFavouriteDto()
    {
        // Arrange
        var id = Guid.NewGuid();
        var workshopId = Guid.NewGuid();
        var favourite = new Favourite { Id = id, WorkshopId = workshopId };
        favouriteRepositoryMock.Setup(repo => repo.GetById(id)).ReturnsAsync(favourite);
        var expectedDto = new FavouriteDto { Id = id, WorkshopId = workshopId };
        mapperMock.Setup(m => m.Map<FavouriteDto>(favourite)).Returns(expectedDto);

        // Act
        var result = await favouriteService.GetById(id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedDto, result);
    }

    [Test]
    public void GetById_NonExistingFavourite_ThrowsArgumentException()
    {
        // Arrange
        var notExistingId = Guid.NewGuid();

        var favouriteRepositoryMock = new Mock<IEntityRepository<Guid, Favourite>>();
        favouriteRepositoryMock
            .Setup(repo => repo.GetById(notExistingId))
            .ReturnsAsync((Favourite)null);

        // Act and Assert
        Assert.ThrowsAsync<ArgumentException>(() => favouriteService.GetById(notExistingId));
    }
}
