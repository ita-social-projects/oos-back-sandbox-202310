using AutoMapper;
using OutOfSchool.WebApi.Models.ChildAchievement;

namespace OutOfSchool.WebApi.Services;

public class ChildAchievementService : IChildAchievementService
{
    private readonly IChildAchievementRepository childAchievementRepository;
    private readonly IChildAchievementTypeRepository childAchievementTypeRepository;
    private readonly IProviderAdminRepository providerAdminRepository;
    private readonly IEntityRepositorySoftDeleted<Guid, OutOfSchool.Services.Models.Child> childRepository;
    private readonly ISensitiveEntityRepositorySoftDeleted<Teacher> teacherRepository;
    private readonly IWorkshopRepository workshopRepository;
    private readonly IApplicationRepository applicationRepository;
    private readonly ILogger<ChildAchievementService> logger;
    private readonly IMapper mapper;

    public ChildAchievementService(
        IChildAchievementRepository childAchievementRepository,
        IChildAchievementTypeRepository childAchievementTypeRepository,
        IProviderAdminRepository providerAdminRepository,
        IEntityRepositorySoftDeleted<Guid, OutOfSchool.Services.Models.Child> childRepository,
        ISensitiveEntityRepositorySoftDeleted<Teacher> teacherRepository,
        IWorkshopRepository workshopRepository,
        IApplicationRepository applicationRepository,
        ILogger<ChildAchievementService> logger,
        IMapper mapper)
    {
        this.childAchievementRepository = childAchievementRepository;
        this.childAchievementTypeRepository = childAchievementTypeRepository;
        this.providerAdminRepository = providerAdminRepository;
        this.childRepository = childRepository;
        this.teacherRepository = teacherRepository;
        this.workshopRepository = workshopRepository;
        this.applicationRepository = applicationRepository;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<ChildAchievementCreationResponseDto> CreateAchievement(ChildAchievementCreationRequestDto childAchievementCreationRequestDto, string userId)
    {
        _ = childAchievementCreationRequestDto ?? throw new ArgumentNullException(nameof(childAchievementCreationRequestDto));

        logger.LogDebug(
            $"Started creation of a new child achievement {nameof(childAchievementCreationRequestDto)}:{childAchievementCreationRequestDto}.");

        var type = (await childAchievementTypeRepository.GetById(childAchievementCreationRequestDto.ChildAchievementTypeId).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to create a new child achievement the Type with " +
                $"{nameof(childAchievementCreationRequestDto.ChildAchievementTypeId)}:{childAchievementCreationRequestDto.ChildAchievementTypeId} was not found.");
        var application = (await applicationRepository.GetById(childAchievementCreationRequestDto.ApplicationId).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to create a new child achievement the Applicaion with " +
                $"{nameof(childAchievementCreationRequestDto.ApplicationId)}:{childAchievementCreationRequestDto.ApplicationId} " +
                $"was not found.");
        var child = (await childRepository.GetById(application.ChildId).ConfigureAwait(false))
                ?? throw new ArgumentException(
                $"Trying to create a new child achievement the Child with " +
                $"{nameof(application.ChildId)}:{application.ChildId} was not found.");
        var workshop = (await workshopRepository.GetById(application.WorkshopId).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to create a new child achievement the Workshop with " +
                $"{nameof(application.WorkshopId)}:{application.WorkshopId} was not found.");

        var admin = (await providerAdminRepository.GetByFilter(p => p.UserId == userId).ConfigureAwait(false)).SingleOrDefault();
        if (admin.ProviderId != workshop.ProviderId)
        {
            throw new UnauthorizedAccessException(
                $"Trying to create a new child the achievement by provider admin wich cant do that.");
        }

        foreach (Teacher t in workshop.Teachers)
        {
            if (t.Id == childAchievementCreationRequestDto.TrainerId)
            {
                var childAchievement = mapper.Map<ChildAchievement>(childAchievementCreationRequestDto);
                childAchievement.Date = DateTime.Now;
                childAchievement.WorkshopId = application.WorkshopId;
                childAchievement.ChildId = application.ChildId;
                var newAchive = await childAchievementRepository.Create(childAchievement);
                logger.LogDebug(
                    $"Child achievement {childAchievementCreationRequestDto} was created successfully.");
                return mapper.Map<ChildAchievementCreationResponseDto>(newAchive);
            }
        }

        throw new ArgumentException(
                $"Trying to create a new child achievement the Workshop teacher with {nameof(childAchievementCreationRequestDto.TrainerId)}:{childAchievementCreationRequestDto.TrainerId} was not found.");
    }

    public async Task DeleteAchievement(Guid id, string userId)
    {
        logger.LogDebug(
            $"Started deleting child achievement with {nameof(id)}:{id}.");

        var achi = (await childAchievementRepository.GetById(id).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to delete not existing Child achievement (Id = {id}).");

        var workshop = (await workshopRepository.GetById(achi.WorkshopId).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to delete a new child achievement the Workshop with " +
                $"{nameof(achi.WorkshopId)}:{achi.WorkshopId} was not found.");

        var admin = (await providerAdminRepository.GetByFilter(p => p.UserId == userId).ConfigureAwait(false)).SingleOrDefault();
        if (admin.ProviderId != workshop.ProviderId)
        {
            throw new UnauthorizedAccessException(
                $"Trying to delete child the achievement by provider admin wich cant do that.");
        }

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

    public async Task<ChildAchievementUpdatingResponseDto> UpdateAchievement(ChildAchievementUpdatingRequestDto childAchievementUpdatingRequestDto, string userId)
    {
        logger.LogDebug(
            $"Started updation of a new child achievement {nameof(childAchievementUpdatingRequestDto)}:{childAchievementUpdatingRequestDto}.");
        _ = childAchievementUpdatingRequestDto ?? throw new ArgumentNullException(nameof(childAchievementUpdatingRequestDto));
        var type = (await childAchievementTypeRepository.GetById(childAchievementUpdatingRequestDto.ChildAchievementTypeId).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to create a new child achievement the Type with " +
                $"{nameof(childAchievementUpdatingRequestDto.ChildAchievementTypeId)}:{childAchievementUpdatingRequestDto.ChildAchievementTypeId} was not found.");
        var application = (await applicationRepository.GetById(childAchievementUpdatingRequestDto.ApplicationId).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to update child achievement the Applicaion with " +
                $"{nameof(childAchievementUpdatingRequestDto.ApplicationId)}:{childAchievementUpdatingRequestDto.ApplicationId} was not found.");
        var child = (await childRepository.GetById(application.ChildId).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to update child achievement the Child with " +
                $"{nameof(application.ChildId)}:{application.ChildId} was not found.");
        var workshop = (await workshopRepository.GetById(application.WorkshopId).ConfigureAwait(false))
            ?? throw new ArgumentException(
                $"Trying to update child achievement the Workshop with " +
                $"{nameof(application.WorkshopId)}:{application.WorkshopId} was not found.");

        var admin = (await providerAdminRepository.GetByFilter(p => p.UserId == userId).ConfigureAwait(false)).SingleOrDefault();
        if (admin.ProviderId != workshop.ProviderId)
        {
            throw new UnauthorizedAccessException(
                $"Trying to update child the achievement by provider admin wich cant do that.");
        }

        foreach (Teacher t in workshop.Teachers)
        {
            if (t.Id == childAchievementUpdatingRequestDto.TrainerId)
            {
                var childAchievement = mapper.Map<ChildAchievement>(childAchievementUpdatingRequestDto);
                childAchievement.WorkshopId = application.WorkshopId;
                childAchievement.ChildId = application.ChildId;
                var updatedAchive = await childAchievementRepository.Update(childAchievement);
                logger.LogDebug(
                $"Child achievement {childAchievementUpdatingRequestDto} was updated successfully.");
                return mapper.Map<ChildAchievementUpdatingResponseDto>(updatedAchive);
            }
        }

        throw new ArgumentException(
                $"Trying to update child achievement the Workshop teacher with {nameof(childAchievementUpdatingRequestDto.TrainerId)}:{childAchievementUpdatingRequestDto.TrainerId} was not found.");
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
