using AutoMapper;
using Nest;
using OutOfSchool.Services.Enums;
using OutOfSchool.WebApi.Common;
using OutOfSchool.WebApi.Models.Application;
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
    private readonly ITeacherService teacherService;
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
        ITeacherService teacherService,
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
        this.teacherService = teacherService;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<Result<ChildAchievementCreationResponseDto>> CreateAchievement(ChildAchievementCreationRequestDto childAchievementCreationRequestDto, string userId)
    {
        _ = childAchievementCreationRequestDto ?? throw new ArgumentNullException(nameof(childAchievementCreationRequestDto));

        logger.LogDebug(
            $"Started creation of a new child achievement {nameof(childAchievementCreationRequestDto)}:{childAchievementCreationRequestDto}.");

        var type = await childAchievementTypeRepository.GetById(childAchievementCreationRequestDto.ChildAchievementTypeId).ConfigureAwait(false);
        if (type is null)
        {
            return Result<ChildAchievementCreationResponseDto>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to create a new child achievement " +
                $"the {nameof(childAchievementCreationRequestDto.ChildAchievementTypeId)}" +
                $": {childAchievementCreationRequestDto.ChildAchievementTypeId} was not found.",
            });
        }

        var application = await applicationRepository.GetById(childAchievementCreationRequestDto.ApplicationId).ConfigureAwait(false);
        if (application is null)
        {
            return Result<ChildAchievementCreationResponseDto>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to create a new child achievement the Application with " +
                $"{nameof(childAchievementCreationRequestDto.ApplicationId)}: {childAchievementCreationRequestDto.ApplicationId} " +
                $"was not found.",
            });
        }

        if (application.Status != ApplicationStatus.Approved || application.Status != ApplicationStatus.StudyingForYears)
        {
            return Result<ChildAchievementCreationResponseDto>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to create a new child achievement the Application status " +
                $"{nameof(application.Status)}: {application.Status} " +
                $"is unacceptable.",
            });
        }

        var child = await childRepository.GetById(application.ChildId).ConfigureAwait(false);
        if (child is null)
        {
            return Result<ChildAchievementCreationResponseDto>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to create a new child Achievement the Child with " +
                $"{nameof(application.ChildId)}:{application.ChildId} was not found.",
            });
        }

        var workshop = await workshopRepository.GetById(application.WorkshopId).ConfigureAwait(false);
        if (workshop is null)
        {
            return Result<ChildAchievementCreationResponseDto>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to create a new child achievement the Workshop with " +
                $"{nameof(application.WorkshopId)}:{application.WorkshopId} was not found.",
            });
        }

        var admin = (await providerAdminRepository.GetByFilter(p => p.UserId == userId).ConfigureAwait(false)).SingleOrDefault();
        if (admin.ProviderId != workshop.ProviderId)
        {
            throw new UnauthorizedAccessException(
                $"Trying to create child the achievement by provider admin wich cant do that.");
        }

        foreach (Teacher t in workshop.Teachers)
        {
            if (t.Id == childAchievementCreationRequestDto.TrainerId)
            {
                var childAchievement = mapper.Map<ChildAchievement>(childAchievementCreationRequestDto);
                childAchievement.Date = DateTime.Now;
                childAchievement.ChildId = application.ChildId;
                var newAchive = await childAchievementRepository.Create(childAchievement);
                logger.LogDebug(
                    $"Child achievement {childAchievementCreationRequestDto} was created successfully.");
                return Result<ChildAchievementCreationResponseDto>.Success(mapper.Map<ChildAchievementCreationResponseDto>(newAchive));
            }
        }

        return Result<ChildAchievementCreationResponseDto>.Failed(new OperationError
        {
            Code = "400",
            Description = $"Trying to create a new child achievement the Workshop teacher with " +
            $"{nameof(childAchievementCreationRequestDto.TrainerId)}:{childAchievementCreationRequestDto.TrainerId} " +
            $"was not found.",
        });
    }

    public async Task<Result<object>> DeleteAchievement(Guid id, string userId)
    {
        logger.LogDebug(
            $"Started deleting child achievement with {nameof(id)}:{id}.");

        var achi = await childAchievementRepository.GetById(id).ConfigureAwait(false);
        if (achi is null)
        {
            return Result<object>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to delete not existing Child achievement (Id = {id}).",
            });
        }

        var admin = (await providerAdminRepository.GetByFilter(p => p.UserId == userId).ConfigureAwait(false)).SingleOrDefault();
        var workshop = await workshopRepository.GetById(await teacherService.GetTeachersWorkshopId(achi.TrainerId));
        if (admin.ProviderId != workshop.ProviderId)
        {
            throw new UnauthorizedAccessException(
                $"Trying to delete child the achievement by provider admin wich cant do that.");
        }

        await childAchievementRepository.DeleteById(id);
        logger.LogDebug(
                $"Child achievement with Id:{id} was created successfully.");
        return Result<object>.Success(null);
    }

    public async Task<Result<IEnumerable<ChildAchievementGettingDto>>> GetAchievementForChildId(Guid id)
    {
        logger.LogDebug(
            $"Started getting child achievement with {nameof(id)}:{id}.");
        var child = await childRepository.GetById(id).ConfigureAwait(false);
        if (child is null)
        {
            return Result<IEnumerable<ChildAchievementGettingDto>>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to get child achievement the Child with {nameof(id)}:{id} was not found.",
            });
        }

        var childAchievements = await childAchievementRepository.GetForChild(id);
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
                $"Child achievements with child Id:{id} was successfully finded.");
        return Result<IEnumerable<ChildAchievementGettingDto>>.Success(childAchievementsDto);
    }

    public async Task<Result<IEnumerable<ChildAchievementGettingDto>>> GetAchievementForWorkshopId(Guid id)
    {
        logger.LogDebug(
            $"Started getting child achievement with {nameof(id)}:{id}.");
        var workshop = await workshopRepository.GetById(id).ConfigureAwait(false);
        if (workshop is null)
        {
            return Result<IEnumerable<ChildAchievementGettingDto>>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to get workshop childs achievements the Workshop with {nameof(id)}:{id} was not found.",
            });
        }

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
        return Result<IEnumerable<ChildAchievementGettingDto>>.Success(childAchievementsDto);
    }

    public async Task<Result<IEnumerable<ChildAchievementGettingDto>>> GetAchievementForWorkshopIdChildId(Guid childId, Guid workshopId)
    {
        logger.LogDebug(
            $"Started getting child achievement with {nameof(childId)}:{childId}, {nameof(workshopId)}:{workshopId}.");
        var child = await childRepository.GetById(childId).ConfigureAwait(false);
        if (child is null)
        {
            return Result<IEnumerable<ChildAchievementGettingDto>>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to get child achievement the Child with {nameof(childId)}:{childId} was not found.",
            });
        }

        var workshop = await workshopRepository.GetById(workshopId).ConfigureAwait(false);
        if (workshop is null)
        {
            return Result<IEnumerable<ChildAchievementGettingDto>>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to get workshop childs achievements the Workshop with {nameof(workshopId)}:{workshopId} was not found.",
            });
        }

        var application = await applicationRepository.GetForWorkshopChild(childId, workshopId).ConfigureAwait(false);
        if (application is null)
        {
            return Result<IEnumerable<ChildAchievementGettingDto>>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to get childs achievement the Applicaion with " +
                $"{nameof(childId)}:{childId} and " +
                $"{nameof(workshopId)}:{workshopId}  was not found.",
            });
        }

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
        return Result<IEnumerable<ChildAchievementGettingDto>>.Success(childAchievementsDto);
    }

    public async Task<Result<ChildAchievementUpdatingResponseDto>> UpdateAchievement(ChildAchievementUpdatingRequestDto childAchievementUpdatingRequestDto, string userId)
    {
        logger.LogDebug(
            $"Started updation of a new child achievement {nameof(childAchievementUpdatingRequestDto)}:{childAchievementUpdatingRequestDto}.");
        _ = childAchievementUpdatingRequestDto ?? throw new ArgumentNullException(nameof(childAchievementUpdatingRequestDto));

        var type = await childAchievementTypeRepository.GetById(childAchievementUpdatingRequestDto.ChildAchievementTypeId).ConfigureAwait(false);
        if (type is null)
        {
            return Result<ChildAchievementUpdatingResponseDto>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to update child achievement the Type with " +
                $"{nameof(childAchievementUpdatingRequestDto.ChildAchievementTypeId)}:{childAchievementUpdatingRequestDto.ChildAchievementTypeId}" +
                $" was not found.",
            });
        }

        var application = await applicationRepository.GetById(childAchievementUpdatingRequestDto.ApplicationId).ConfigureAwait(false);
        if (application is null)
        {
            return Result<ChildAchievementUpdatingResponseDto>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to update child achievement the Applicaion with " +
                $"{nameof(childAchievementUpdatingRequestDto.ApplicationId)}:{childAchievementUpdatingRequestDto.ApplicationId} was not found.",
            });
        }

        var child = await childRepository.GetById(application.ChildId).ConfigureAwait(false);
        if (child is null)
        {
            return Result<ChildAchievementUpdatingResponseDto>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to update child achievement the Child with " +
                $"{nameof(application.ChildId)}:{application.ChildId} was not found.",
            });
        }

        var workshop = await workshopRepository.GetById(application.WorkshopId).ConfigureAwait(false);
        if (child is null)
        {
            return Result<ChildAchievementUpdatingResponseDto>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to update child achievement the Workshop with " +
                $"{nameof(application.WorkshopId)}:{application.WorkshopId} was not found.",
            });
        }

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
                childAchievement.ChildId = application.ChildId;
                var updatedAchive = await childAchievementRepository.Update(childAchievement);
                logger.LogDebug(
                $"Child achievement {childAchievementUpdatingRequestDto} was updated successfully.");
                return Result<ChildAchievementUpdatingResponseDto>.Success(mapper.Map<ChildAchievementUpdatingResponseDto>(updatedAchive));
            }
        }

        return Result<ChildAchievementUpdatingResponseDto>.Failed(new OperationError
        {
            Code = "400",
            Description = $"Trying to update child achievement the Workshop teacher with " +
            $"{nameof(childAchievementUpdatingRequestDto.TrainerId)}:{childAchievementUpdatingRequestDto.TrainerId} " +
            $"was not found.",
        });
    }

    public async Task<Result<IEnumerable<ChildAchievementGettingDto>>> GetAll()
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
        return Result<IEnumerable<ChildAchievementGettingDto>>.Success(childAchievementsDto);
    }
}
