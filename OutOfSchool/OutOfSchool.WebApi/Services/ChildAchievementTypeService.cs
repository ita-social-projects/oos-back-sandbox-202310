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

    public ChildAchievementTypeService(IChildAchievementTypeRepository childAchievementTypeRepository, ILogger<ChildAchievementTypeService> logger, IMapper mapper)
    {
        this.childAchievementTypeRepository = childAchievementTypeRepository;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<ChildAchievementType> CreateAchievementType(ChildAchievementTypeRequestDto childAchievementTypeRequestDto)
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

    public async Task DeleteAchievementType(int id)
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

    public async Task<IEnumerable<ChildAchievementType>> GetAllAchievementTypes()
    {
        logger.LogDebug(
            $"Started getting all child achievement types.");
        var childAchievementsTypes = await childAchievementTypeRepository.GetAll();
        logger.LogDebug(
                $"All child achievements was successfully finded.");
        return childAchievementsTypes;
    }

    public async Task<ChildAchievementType> GetAchievementTypeById(int id)
    {
        logger.LogDebug(
            $"Started getting child achievement type with{nameof(id)}:{id}.");
        var childAchievementsType = await childAchievementTypeRepository.GetById(id);
        logger.LogDebug(
                $"Child achievement type was successfully finded.");
        return childAchievementsType;
    }
}
