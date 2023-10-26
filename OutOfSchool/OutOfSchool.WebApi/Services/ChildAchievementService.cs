using AutoMapper;
using Elastic.CommonSchema;
using Microsoft.Extensions.Options;
using Nest;
using OutOfSchool.WebApi.Models;

namespace OutOfSchool.WebApi.Services;

public class ChildAchievementService : IChildAchievementService
{
    private readonly IChildAchievementRepository childAchievementRepository;
    private readonly ILogger<ChildAchievementService> logger;
    private readonly IMapper mapper;

    public ChildAchievementService(
        IChildAchievementRepository childAchievementRepository,
        ILogger<ChildAchievementService> logger,
        IMapper mapper)
    {
        this.childAchievementRepository = childAchievementRepository;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<ChildAchievementCreationDto> CreateAchievement(ChildAchievementCreationDto childAchievementCreationDto)
    {
        _ = childAchievementCreationDto ?? throw new ArgumentNullException(nameof(childAchievementCreationDto));
        var childAchievement = mapper.Map<ChildAchievement>(childAchievementCreationDto);
        var newAchive = await childAchievementRepository.Create(childAchievement);
        return mapper.Map<ChildAchievementCreationDto>(newAchive);
    }

    public async Task DeleteAchievement(Guid id)
    {
        await childAchievementRepository.Delete(id);
    }

    public async Task<IEnumerable<ChildAchievementDto>> GetAchievementForChildId(Guid id)
    {
        var childAchievements = await childAchievementRepository.GetForChild(id);
        List < ChildAchievementDto > childAchievementsDto= new List<ChildAchievementDto>();
        foreach (ChildAchievement ch in childAchievements) {
            childAchievementsDto.Add(mapper.Map<ChildAchievementDto>(ch));
        }

        return childAchievementsDto;
    }

    public async Task<IEnumerable<ChildAchievementDto>> GetAchievementForWorkshopId(Guid id)
    {
        var childAchievements = await childAchievementRepository.GetForWorkshop(id);
        List<ChildAchievementDto> childAchievementsDto = new List<ChildAchievementDto>();
        foreach (ChildAchievement ch in childAchievements)
        {
            childAchievementsDto.Add(mapper.Map<ChildAchievementDto>(ch));
        }

        return childAchievementsDto;
    }

    public async Task<IEnumerable<ChildAchievementDto>> GetAchievementForWorkshopIdChildId(Guid childId, Guid workshopId)
    {
        var childAchievements = await childAchievementRepository.GetForWorkshopChild(childId, workshopId);
        List<ChildAchievementDto> childAchievementsDto = new List<ChildAchievementDto>();
        foreach (ChildAchievement ch in childAchievements)
        {
            childAchievementsDto.Add(mapper.Map<ChildAchievementDto>(ch));
        }

        return childAchievementsDto;
    }

    public async Task<ChildAchievementDto> UpdateAchievement(ChildAchievementDto childAchievementDto)
    {
        _ = childAchievementDto ?? throw new ArgumentNullException(nameof(childAchievementDto));
        var childAchievement = mapper.Map<ChildAchievement>(childAchievementDto);
        var updatedAchive = await childAchievementRepository.Update(childAchievement);
        return mapper.Map<ChildAchievementDto>(updatedAchive);
    }
}
