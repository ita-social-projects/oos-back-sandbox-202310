using AutoMapper;
using OutOfSchool.Services.Models;
using OutOfSchool.WebApi.Common;
using OutOfSchool.WebApi.Models.ChildAchievement;

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

    public async Task<Result<ChildAchievementType>> CreateAchievementType(ChildAchievementTypeRequestDto childAchievementTypeRequestDto)
    {
        _ = childAchievementTypeRequestDto ?? throw new ArgumentNullException(nameof(childAchievementTypeRequestDto));

        logger.LogDebug(
            $"Started creation of a new child achievement type {nameof(childAchievementTypeRequestDto)}:{childAchievementTypeRequestDto}.");

        var allAchiveTypes = await childAchievementTypeRepository.GetAll();
        foreach (ChildAchievementType chT in allAchiveTypes)
        {
            if (chT.Type == childAchievementTypeRequestDto.Type) 
            {
                return Result<ChildAchievementType>.Failed(new OperationError
                {
                    Code = "400",
                    Description = $"Trying to create a new child achievement type with same type wich is already exist" +
                        $"{nameof(childAchievementTypeRequestDto.Type)}:{childAchievementTypeRequestDto.Type}.",
                });
            }
        }

        var childAchiveType = mapper.Map<ChildAchievementType>(childAchievementTypeRequestDto);
        var newAchiveType = await childAchievementTypeRepository.Create(childAchiveType);
        logger.LogDebug(
            $"Child achievement type {newAchiveType} was created successfully.");
        return Result<ChildAchievementType>.Success(newAchiveType);
    }

    public async Task<Result<object>> DeleteAchievementType(int id)
    {
        logger.LogDebug(
            $"Started deleting child achievement type with {nameof(id)}:{id}.");

        var achiT = (await childAchievementTypeRepository.GetById(id).ConfigureAwait(false));
        if (achiT is null)
        {
            return Result<object>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to delete not existing achievement type (Id = {id}).",
            });
        }

        await childAchievementTypeRepository.DeleteById(id);
        logger.LogDebug(
                $"Child achievement type with Id:{id} was created successfully.");
        return Result<object>.Success(null);
    }

    public async Task<Result<IEnumerable<ChildAchievementType>>> GetAllAchievementTypes()
    {
        logger.LogDebug(
            $"Started getting all child achievement types.");
        var childAchievementsTypes = await childAchievementTypeRepository.GetAll();
        logger.LogDebug(
                $"All child achievements was successfully finded.");
        return Result<IEnumerable<ChildAchievementType>>.Success(childAchievementsTypes);
    }

    public async Task<Result<ChildAchievementType>> GetAchievementTypeById(int id)
    {
        logger.LogDebug(
            $"Started getting child achievement type with{nameof(id)}:{id}.");
        var childAchievementsType = await childAchievementTypeRepository.GetById(id);
        logger.LogDebug(
                $"Child achievement type was successfully finded.");
        return Result<ChildAchievementType>.Success(childAchievementsType);
    }
}
