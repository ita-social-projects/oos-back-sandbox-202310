using AutoMapper;
using Elastic.CommonSchema;
using Microsoft.Extensions.Options;
using Nest;
using OutOfSchool.Services.Models;
using OutOfSchool.WebApi.Models;
using System.Security.Cryptography;

namespace OutOfSchool.WebApi.Services;

public class ChildAchievementService : IChildAchievementService
{
    private readonly IChildAchievementRepository childAchievementRepository;
    private readonly IEntityRepositorySoftDeleted<Guid, Child> childRepository;
    private protected readonly IWorkshopRepository workshopRepository;
    private readonly ILogger<ChildAchievementService> logger;
    private readonly IMapper mapper;

    public ChildAchievementService(
        IChildAchievementRepository childAchievementRepository,
        IEntityRepositorySoftDeleted<Guid, Child> childRepository,
        IWorkshopRepository workshopRepository,
        ILogger<ChildAchievementService> logger,
        IMapper mapper)
    {
        this.childAchievementRepository = childAchievementRepository;
        this.childRepository = childRepository;
        this.workshopRepository = workshopRepository;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<ChildAchievementCreationDto> CreateAchievement(ChildAchievementCreationDto childAchievementCreationDto)
    {
        _ = childAchievementCreationDto ?? throw new ArgumentNullException(nameof(childAchievementCreationDto));

        var child = (await childRepository.GetById(childAchievementCreationDto.ChildId).ConfigureAwait(false))
            ?? throw new UnauthorizedAccessException(
                $"Trying to create a new child achievement the Child with {nameof(childAchievementCreationDto.ChildId)}:{childAchievementCreationDto.ChildId} was not found.");
        var workshop = (await workshopRepository.GetById(childAchievementCreationDto.WorkshopId).ConfigureAwait(false))
            ?? throw new UnauthorizedAccessException(
                $"Trying to create a new child achievement the Workshop with {nameof(childAchievementCreationDto.WorkshopId)}:{childAchievementCreationDto.WorkshopId} was not found.");
        foreach (Teacher t in workshop.Teachers) {
            if (string.Format(t.LastName + " " + t.FirstName + " " + t.MiddleName) == childAchievementCreationDto.Trainer)
            {
                var childAchievement = mapper.Map<ChildAchievement>(childAchievementCreationDto);
                var newAchive = await childAchievementRepository.Create(childAchievement);
                return mapper.Map<ChildAchievementCreationDto>(newAchive);
            }
        }

        throw new UnauthorizedAccessException(
                $"Trying to create a new child achievement the Workshop teacher with {nameof(childAchievementCreationDto.Trainer)}:{childAchievementCreationDto.Trainer} was not found.");
    }

    public async Task DeleteAchievement(Guid id)
    {
        var achi = (await childAchievementRepository.GetById(id).ConfigureAwait(false))
            ?? throw new UnauthorizedAccessException(
                $"Trying to delete not existing Child (Id = {id}).");
        await childAchievementRepository.Delete(id);
    }

    public async Task<IEnumerable<ChildAchievementDto>> GetAchievementForChildId(Guid id)
    {
        var child = (await childRepository.GetById(id).ConfigureAwait(false))
            ?? throw new UnauthorizedAccessException(
                $"Trying to get child achievement the Child with {nameof(id)}:{id} was not found.");
        var childAchievements = await childAchievementRepository.GetForChild(id);
        List < ChildAchievementDto > childAchievementsDto = new List<ChildAchievementDto>();
        foreach (ChildAchievement ch in childAchievements) 
        {
            childAchievementsDto.Add(mapper.Map<ChildAchievementDto>(ch));
        }

        return childAchievementsDto;
    }

    public async Task<IEnumerable<ChildAchievementDto>> GetAchievementForWorkshopId(Guid id)
    {
        var workshop = (await workshopRepository.GetById(id).ConfigureAwait(false))
            ?? throw new UnauthorizedAccessException(
                $"Trying to get workshop childs achievements the Workshop with {nameof(id)}:{id} was not found.");
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
        var child = (await childRepository.GetById(childId).ConfigureAwait(false))
            ?? throw new UnauthorizedAccessException(
                $"Trying to get child achievement the Child with {nameof(childId)}:{childId} was not found.");
        var workshop = (await workshopRepository.GetById(workshopId).ConfigureAwait(false))
            ?? throw new UnauthorizedAccessException(
                $"Trying to get workshop childs achievements the Workshop with {nameof(workshopId)}:{workshopId} was not found.");
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
        var child = (await childRepository.GetById(childAchievementDto.ChildId).ConfigureAwait(false))
            ?? throw new UnauthorizedAccessException(
                $"Trying to update child achievement the Child with {nameof(childAchievementDto.ChildId)}:{childAchievementDto.ChildId} was not found.");
        var workshop = (await workshopRepository.GetById(childAchievementDto.WorkshopId).ConfigureAwait(false))
            ?? throw new UnauthorizedAccessException(
                $"Trying to update child achievement the Workshop with {nameof(childAchievementDto.WorkshopId)}:{childAchievementDto.WorkshopId} was not found.");
        foreach (Teacher t in workshop.Teachers)
        {
            if (string.Format(t.LastName + " " + t.FirstName + " " + t.MiddleName) == childAchievementDto.Trainer)
            {
                var childAchievement = mapper.Map<ChildAchievement>(childAchievementDto);
                var updatedAchive = await childAchievementRepository.Update(childAchievement);
                return mapper.Map<ChildAchievementDto>(updatedAchive);
            }
        }

        throw new UnauthorizedAccessException(
                $"Trying to update child achievement the Workshop teacher with {nameof(childAchievementDto.Trainer)}:{childAchievementDto.Trainer} was not found.");
    }
}
