using AutoMapper;
using Nest;
using OutOfSchool.Services.Models;
using OutOfSchool.Services.Repository;
using OutOfSchool.WebApi.Models.ChildAchievement;
using static System.Net.Mime.MediaTypeNames;

namespace OutOfSchool.WebApi.Services;

public class ChildAchievementTypeService : IChildAchievementTypeService
{
    private readonly IChildAchievementTypeRepository childAchievementTypeRepository;
    private readonly ILogger<ChildAchievementTypeService> logger;
    private readonly IMapper mapper;

    public ChildAchievementTypeService(IChildAchievementTypeRepository childAchievementTypeRepository, ILogger<ChildAchievementTypeService> logger)
    {
        this.childAchievementTypeRepository = childAchievementTypeRepository;
        this.logger = logger;
    }

    public async Task<ChildAchievementType> Create(ChildAchievementTypeRequestDto childAchievementTypeRequestDto)
    {
        _ = childAchievementTypeRequestDto ?? throw new ArgumentNullException(nameof(childAchievementTypeRequestDto));

        logger.LogDebug(
            $"Started creation of a new child achievement type {nameof(childAchievementTypeRequestDto)}:{childAchievementTypeRequestDto}.");

        var allAchiveTypes = await childAchievementTypeRepository.GetAll();
        foreach (ChildAchievementType chT in allAchiveTypes)
        {
            if (chT.Type == childAchievementTypeRequestDto.Type) {
                throw new ArgumentException(
                $"Trying to create a new child achievement type with same type wich is already exist" +
                $"{nameof(childAchievementTypeRequestDto.Type)}:{childAchievementTypeRequestDto.Type}.");
            }
        }

        var childAchiveType = mapper.Map<ChildAchievementType>(childAchievementTypeRequestDto);
        var newAchiveType = await childAchievementTypeRepository.Create(childAchiveType);
        logger.LogDebug(
            $"Child achievement type {newAchiveType} was created successfully.");
        return newAchiveType;
    }

    public async Task Delete(int id)
    {
        logger.LogDebug(
            $"Started deleting child achievement type with {nameof(id)}:{id}.");

        var achiT = (await childAchievementTypeRepository.GetById(id).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to delete not existing achievement type (Id = {id}).");

        await childAchievementTypeRepository.Delete(id);
        logger.LogDebug(
                $"Child achievement type with Id:{id} was created successfully.");
    }

    public async Task<IEnumerable<ChildAchievementType>> GetAll()
    {
        logger.LogDebug(
            $"Started getting all child achievement types.");
        var childAchievementsTypes = await childAchievementTypeRepository.GetAll();
        logger.LogDebug(
                $"All child achievements was successfully finded.");
        return childAchievementsTypes;
    }

    public async Task<ChildAchievementType> GetById(int id)
    {
        logger.LogDebug(
            $"Started getting child achievement type with{nameof(id)}:{id}.");
        var childAchievementsType = await childAchievementTypeRepository.GetById(id);
        logger.LogDebug(
                $"Child achievement type was successfully finded.");
        return childAchievementsType;
    }

    public async Task<ChildAchievementType> Update(ChildAchievementType childAchievementType)
    {
        logger.LogDebug(
            $"Started updation of a new child achievement {nameof(childAchievementType)}:{childAchievementType}.");
        _ = childAchievementType ?? throw new ArgumentNullException(nameof(childAchievementType));

        var childAchievementsType = await childAchievementTypeRepository.GetById(childAchievementType.Id)
            ?? throw new ArgumentException(
                $"Trying to update child achievement type " +
                $"{nameof(childAchievementType)}:{childAchievementType} was not found.");

        var allAchiveTypes = await childAchievementTypeRepository.GetAll();
        foreach (ChildAchievementType chT in allAchiveTypes)
        {
            if (chT.Type == childAchievementType.Type)
            {
                throw new ArgumentException(
                $"Trying to update child achievement type with same type wich is already exist" +
                $"{nameof(childAchievementType.Type)}:{childAchievementType.Type}.");
            }
        }
        var updatedAchiveType = await childAchievementTypeRepository.Update(childAchievementType);
        logger.LogDebug(
            $"Child achievement type {childAchievementType} was updated successfully.");
        return updatedAchiveType;
    }
}
