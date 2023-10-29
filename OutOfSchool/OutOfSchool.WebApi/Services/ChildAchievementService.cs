using AutoMapper;
using Elastic.CommonSchema;
using Microsoft.Extensions.Options;
using Nest;
using OutOfSchool.ElasticsearchData.Models;
using OutOfSchool.Services.Models;
using OutOfSchool.WebApi.Models;
using System.Security.Cryptography;
using static Nest.JoinField;

namespace OutOfSchool.WebApi.Services;

public class ChildAchievementService : IChildAchievementService
{
    private readonly IChildAchievementRepository childAchievementRepository;
    private readonly IChildAchievementTypeRepository childAchievementTypeRepository;
    private readonly IEntityRepositorySoftDeleted<Guid, OutOfSchool.Services.Models.Child> childRepository;
    private readonly ISensitiveEntityRepositorySoftDeleted<Teacher> teacherRepository;
    private readonly IWorkshopRepository workshopRepository;
    private readonly IApplicationRepository applicationRepository;
    private readonly ILogger<ChildAchievementService> logger;
    private readonly IMapper mapper;

    public ChildAchievementService(
        IChildAchievementRepository childAchievementRepository,
        IChildAchievementTypeRepository childAchievementTypeRepository,
        IEntityRepositorySoftDeleted<Guid, OutOfSchool.Services.Models.Child> childRepository,
        ISensitiveEntityRepositorySoftDeleted<Teacher> teacherRepository,
        IWorkshopRepository workshopRepository,
        IApplicationRepository applicationRepository,
        ILogger<ChildAchievementService> logger,
        IMapper mapper)
    {
        this.childAchievementRepository = childAchievementRepository;
        this.childAchievementTypeRepository = childAchievementTypeRepository;
        this.childRepository = childRepository;
        this.teacherRepository = teacherRepository;
        this.workshopRepository = workshopRepository;
        this.applicationRepository = applicationRepository;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<ChildAchievementCreationDto> CreateAchievement(ChildAchievementCreationDto childAchievementCreationDto)
    {
        _ = childAchievementCreationDto ?? throw new ArgumentNullException(nameof(childAchievementCreationDto));

        logger.LogDebug(
            $"Started creation of a new child achievement {nameof(childAchievementCreationDto)}:{childAchievementCreationDto}.");

        var type = (await childAchievementTypeRepository.GetById(childAchievementCreationDto.ChildAchievementTypeId).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to create a new child achievement the Type with " +
                $"{nameof(childAchievementCreationDto.ChildAchievementTypeId)}:{childAchievementCreationDto.ChildAchievementTypeId} was not found.");
        var child = (await childRepository.GetById(childAchievementCreationDto.ChildId).ConfigureAwait(false))
                ?? throw new ArgumentException(
                $"Trying to create a new child achievement the Child with " +
                $"{nameof(childAchievementCreationDto.ChildId)}:{childAchievementCreationDto.ChildId} was not found.");
        var workshop = (await workshopRepository.GetById(childAchievementCreationDto.WorkshopId).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to create a new child achievement the Workshop with " +
                $"{nameof(childAchievementCreationDto.WorkshopId)}:{childAchievementCreationDto.WorkshopId} was not found.");
        var application = (await applicationRepository.GetForWorkshopChild(childAchievementCreationDto.ChildId, childAchievementCreationDto.WorkshopId).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to create a new child achievement the Applicaion with " +
                $"{nameof(childAchievementCreationDto.ChildId)}:{childAchievementCreationDto.ChildId} and " +
                $"{nameof(childAchievementCreationDto.WorkshopId)}:{childAchievementCreationDto.WorkshopId}  was not found.");
        foreach (Teacher t in workshop.Teachers)
        {
            if (t.Id == childAchievementCreationDto.TrainerId)
            {
                var childAchievement = mapper.Map<ChildAchievement>(childAchievementCreationDto);
                childAchievement.Date = DateTime.Now;
                var newAchive = await childAchievementRepository.Create(childAchievement);
                logger.LogDebug(
                    $"Child achievement {childAchievementCreationDto} was created successfully.");
                return mapper.Map<ChildAchievementCreationDto>(newAchive);
            }
        }

        throw new ArgumentException(
                $"Trying to create a new child achievement the Workshop teacher with {nameof(childAchievementCreationDto.TrainerId)}:{childAchievementCreationDto.TrainerId} was not found.");
    }

    public async Task DeleteAchievement(Guid id)
    {
        logger.LogDebug(
            $"Started deleting child achievement with {nameof(id)}:{id}.");
        var achi = (await childAchievementRepository.GetById(id).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to delete not existing Child (Id = {id}).");
        await childAchievementRepository.Delete(id);
        logger.LogDebug(
                $"Child achievement with Id:{id} was created successfully.");
    }

    public async Task<IEnumerable<ChildAchievementGettingDto>> GetAchievementForChildId(Guid id)
    {
        logger.LogDebug(
            $"Started getting child achievement with {nameof(id)}:{id}.");
        var child = (await childRepository.GetById(id).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to get child achievement the Child with {nameof(id)}:{id} was not found.");
        var childAchievements = await childAchievementRepository.GetForChild(id);
        List <ChildAchievementGettingDto> childAchievementsDto = new List<ChildAchievementGettingDto>();
        foreach (ChildAchievement ch in childAchievements) 
        {
            childAchievementsDto.Add(mapper.Map<ChildAchievementGettingDto>(ch));
            var type = await childAchievementTypeRepository.GetById(ch.ChildAchievementTypeId);
            var teacher = await teacherRepository.GetById(ch.TrainerId);
            childAchievementsDto.Last().Type = type.Type;
            childAchievementsDto.Last().Trainer = string.Format(teacher.FirstName + " " + teacher.LastName + " " + teacher.MiddleName);
        }

        logger.LogDebug(
                $"Child achievements with child Id:{id} was successfully finded.");
        return childAchievementsDto;
    }

    public async Task<IEnumerable<ChildAchievementGettingDto>> GetAchievementForWorkshopId(Guid id)
    {
        logger.LogDebug(
            $"Started getting child achievement with {nameof(id)}:{id}.");
        var workshop = (await workshopRepository.GetById(id).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to get workshop childs achievements the Workshop with {nameof(id)}:{id} was not found.");
        var childAchievements = await childAchievementRepository.GetForWorkshop(id);
        List<ChildAchievementGettingDto> childAchievementsDto = new List<ChildAchievementGettingDto>();
        foreach (ChildAchievement ch in childAchievements)
        {
            childAchievementsDto.Add(mapper.Map<ChildAchievementGettingDto>(ch));
            var type = await childAchievementTypeRepository.GetById(ch.ChildAchievementTypeId);
            var teacher = await teacherRepository.GetById(ch.TrainerId);
            childAchievementsDto.Last().Type = type.Type;
            childAchievementsDto.Last().Trainer = string.Format(teacher.FirstName + " " + teacher.LastName + " " + teacher.MiddleName);
        }

        logger.LogDebug(
                $"Child achievements with workshop Id:{id} was successfully finded.");
        return childAchievementsDto;
    }

    public async Task<IEnumerable<ChildAchievementGettingDto>> GetAchievementForWorkshopIdChildId(Guid childId, Guid workshopId)
    {
        logger.LogDebug(
            $"Started getting child achievement with {nameof(childId)}:{childId}, {nameof(workshopId)}:{workshopId}.");
        var child = (await childRepository.GetById(childId).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to get child achievement the Child with {nameof(childId)}:{childId} was not found.");
        var workshop = (await workshopRepository.GetById(workshopId).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to get workshop childs achievements the Workshop with {nameof(workshopId)}:{workshopId} was not found.");
        var application = (await applicationRepository.GetForWorkshopChild(childId, workshopId).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to get childs achievement the Applicaion with " +
                $"{nameof(childId)}:{childId} and " +
                $"{nameof(workshopId)}:{workshopId}  was not found.");
        var childAchievements = await childAchievementRepository.GetForWorkshopChild(childId, workshopId);
        List<ChildAchievementGettingDto> childAchievementsDto = new List<ChildAchievementGettingDto>();
        foreach (ChildAchievement ch in childAchievements)
        {
            childAchievementsDto.Add(mapper.Map<ChildAchievementGettingDto>(ch));
            var type = await childAchievementTypeRepository.GetById(ch.ChildAchievementTypeId);
            var teacher = await teacherRepository.GetById(ch.TrainerId);
            childAchievementsDto.Last().Type = type.Type;
            childAchievementsDto.Last().Trainer = string.Format(teacher.FirstName + " " + teacher.LastName + " " + teacher.MiddleName);
        }

        logger.LogDebug(
                $"Child achievements with child Id:{childId}, workshop Id:{workshopId} was successfully finded.");
        return childAchievementsDto;
    }

    public async Task<ChildAchievementUpdatingDto> UpdateAchievement(ChildAchievementUpdatingDto childAchievementDto)
    {
        logger.LogDebug(
            $"Started updation of a new child achievement {nameof(childAchievementDto)}:{childAchievementDto}.");
        _ = childAchievementDto ?? throw new ArgumentNullException(nameof(childAchievementDto));
        var type = (await childAchievementTypeRepository.GetById(childAchievementDto.ChildAchievementTypeId).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to create a new child achievement the Type with " +
                $"{nameof(childAchievementDto.ChildAchievementTypeId)}:{childAchievementDto.ChildAchievementTypeId} was not found.");
        var child = (await childRepository.GetById(childAchievementDto.ChildId).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to update child achievement the Child with " +
                $"{nameof(childAchievementDto.ChildId)}:{childAchievementDto.ChildId} was not found.");
        var workshop = (await workshopRepository.GetById(childAchievementDto.WorkshopId).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to update child achievement the Workshop with " +
                $"{nameof(childAchievementDto.WorkshopId)}:{childAchievementDto.WorkshopId} was not found.");
        var application = (await applicationRepository.GetForWorkshopChild(childAchievementDto.ChildId, childAchievementDto.WorkshopId).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to update child achievement the Applicaion with " +
                $"{nameof(childAchievementDto.ChildId)}:{childAchievementDto.ChildId} and " +
                $"{nameof(childAchievementDto.WorkshopId)}:{childAchievementDto.WorkshopId}  was not found.");
        foreach (Teacher t in workshop.Teachers)
        {
            if (t.Id == childAchievementDto.TrainerId)
            {
                var childAchievement = mapper.Map<ChildAchievement>(childAchievementDto);
                var updatedAchive = await childAchievementRepository.Update(childAchievement);
                logger.LogDebug(
                $"Child achievement {childAchievementDto} was updated successfully.");
                return mapper.Map<ChildAchievementUpdatingDto>(updatedAchive);
            }
        }

        throw new ArgumentException(
                $"Trying to update child achievement the Workshop teacher with {nameof(childAchievementDto.TrainerId)}:{childAchievementDto.TrainerId} was not found.");
    }

    public async Task<IEnumerable<ChildAchievementGettingDto>> GetAll()
    {
        logger.LogDebug(
            $"Started getting all child achievements.");
        var childAchievements = await childAchievementRepository.GetAll();
        List<ChildAchievementGettingDto> childAchievementsDto = new List<ChildAchievementGettingDto>();
        foreach (ChildAchievement ch in childAchievements)
        {
            childAchievementsDto.Add(mapper.Map<ChildAchievementGettingDto>(ch));
            var type = await childAchievementTypeRepository.GetById(ch.ChildAchievementTypeId);
            var teacher = await teacherRepository.GetById(ch.TrainerId);
            childAchievementsDto.Last().Type = type.Type;
            childAchievementsDto.Last().Trainer = string.Format(teacher.FirstName + " " + teacher.LastName + " " + teacher.MiddleName);
        }

        logger.LogDebug(
                $"All child achievements was successfully finded.");
        return childAchievementsDto;
    }
}
