using AutoMapper;
using OutOfSchool.Services.Repository;
using OutOfSchool.WebApi.Common;
using OutOfSchool.WebApi.Models.ChildAchievement;
using OutOfSchool.WebApi.Models.Ministry;
using OutOfSchool.Services.Enums;
using OutOfSchool.Services.Models;

namespace OutOfSchool.WebApi.Services;

public class MinistryAdminService : IMinistryAdminService
{
    private readonly IMinistryAdminRepository ministryAdminRepository;
    private readonly IMinistryRepository ministryRepository;
    private readonly ICodeficatorRepository codeficatorRepository;
    private readonly IMapper mapper;
    private readonly ILogger<MinistryAdminService> logger;

    public MinistryAdminService(
        IMinistryAdminRepository ministryAdminRepository,
        IMinistryRepository ministryRepository,
        ICodeficatorRepository codeficatorRepository,
        IMapper mapper,
        ILogger<MinistryAdminService> logger)
    {
        this.ministryAdminRepository = ministryAdminRepository;
        this.ministryRepository = ministryRepository;
        this.codeficatorRepository = codeficatorRepository;
        this.mapper = mapper;
        this.logger = logger;
    }

    public async Task<Result<object>> Approve(Guid id)
    {
        logger.LogDebug(
            $"Started approving ministry admin {nameof(id)}:{id}.");
        if (await GetById(id) is null)
        {
            return Result<object>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to approve ministry admin with " +
                    $"{nameof(id)}:{id} " +
                    $"was not found.",
            });
        }

        ministryAdminRepository.Approve(id);
        return Result<object>.Success(null);
    }

    public async Task<Result<MinistryAdminCreationResponseDto>> Create(MinistryAdminCreationRequestDto ministryAdminCreationRequestDto)
    {
        logger.LogDebug(
            $"Started creation of a new ministry admin {nameof(ministryAdminCreationRequestDto)}:{ministryAdminCreationRequestDto}.");

        if (await ministryRepository.GetById(ministryAdminCreationRequestDto.MinistryId) is null)
        {
            return Result<MinistryAdminCreationResponseDto>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to create a new ministry admin the Ministry with " +
                $"{nameof(ministryAdminCreationRequestDto.MinistryId)}:{ministryAdminCreationRequestDto.MinistryId} " +
                $"was not found.",
            });
        }

        if (await codeficatorRepository.GetById(ministryAdminCreationRequestDto.SettlementId) is null) {
            return Result<MinistryAdminCreationResponseDto>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to create a new ministry admin the Settlement with " +
                $"{nameof(ministryAdminCreationRequestDto.SettlementId)}:{ministryAdminCreationRequestDto.SettlementId} " +
                $"was not found.",
            });
        }

        var ministryAdmin = mapper.Map<MinistryAdmin>(ministryAdminCreationRequestDto);
        ministryAdmin.Status = MinistryAdminStatus.Pending;

        var ministryAdminDto = mapper.Map<MinistryAdminCreationResponseDto>(await ministryAdminRepository.Create(ministryAdmin));
        ministryAdminDto.Settlement = codeficatorRepository.GetById(ministryAdminCreationRequestDto.SettlementId).Result.Name;
        return Result<MinistryAdminCreationResponseDto>.Success(ministryAdminDto);
    }

    public async Task<Result<object>> Delete(Guid id)
    {
        logger.LogDebug(
            $"Started deleting of a new ministry admin {nameof(id)}:{id}.");

        var ministryAdmin = await ministryAdminRepository.GetById(id).ConfigureAwait(false);
        if (ministryAdmin is null)
        {
            return Result<object>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to delete not existing ministry admin (Id = {id}).",
            });
        }

        await ministryAdminRepository.Delete(ministryAdmin);

        logger.LogDebug(
            $"Ministry admin deleted{nameof(id)}:{id}.");
        return Result<object>.Success(null);
    }

    public async Task<Result<MinistryAdminUpdatingDto>> Update(MinistryAdminUpdatingDto ministryAdminUpdatingDto)
    {
        logger.LogDebug(
            $"Started updating ministry admin {nameof(ministryAdminUpdatingDto)}:{ministryAdminUpdatingDto}.");

        var ministeryAdmin = await GetById(ministryAdminUpdatingDto.Id);
        if (ministeryAdmin is null)
        {
            return Result<MinistryAdminUpdatingDto>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to update ministry admin with " +
                    $"{nameof(ministryAdminUpdatingDto.MinistryId)}:{ministryAdminUpdatingDto.MinistryId} " +
                    $"was not found.",
            });
        }

        if (ministeryAdmin.Value.Status == MinistryAdminStatus.Approved)
        {
            return Result<MinistryAdminUpdatingDto>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to update ministry admin with " +
                        $"{nameof(ministeryAdmin.Value.Status)}:{ministeryAdmin.Value.Status} .",
            });
        }

        if (await ministryRepository.GetById(ministryAdminUpdatingDto.MinistryId) is null)
        {
            return Result<MinistryAdminUpdatingDto>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to update ministry admin the Ministry with " +
                $"{nameof(ministryAdminUpdatingDto.MinistryId)}:{ministryAdminUpdatingDto.MinistryId} " +
                $"was not found.",
            });
        }

        if (await codeficatorRepository.GetById(ministryAdminUpdatingDto.SettlementId) is null)
        {
            return Result<MinistryAdminUpdatingDto>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to create a new ministry admin the Settlement with " +
                $"{nameof(ministryAdminUpdatingDto.SettlementId)}:{ministryAdminUpdatingDto.SettlementId} " +
                $"was not found.",
            });
        }

        var ministeryUpdAdmin = mapper.Map<MinistryAdmin>(ministryAdminUpdatingDto);
        ministeryUpdAdmin.Status = MinistryAdminStatus.Pending;

        return Result<MinistryAdminUpdatingDto>.Success(mapper.Map<MinistryAdminUpdatingDto>(
            await ministryAdminRepository.Update(ministeryUpdAdmin)));
    }

    public async Task<Result<IEnumerable<MinistryAdminGettingDto>>> GetAll()
    {
        logger.LogDebug(
            $"Started getting all ministry admins.");
        var ministeryAdmins = await ministryAdminRepository.GetAll();
        List<MinistryAdminGettingDto> ministeryAdminDtos = new List<MinistryAdminGettingDto>();
        foreach (MinistryAdmin ma in ministeryAdmins) {
            ministeryAdminDtos.Add(mapper.Map<MinistryAdminGettingDto>(ma));
            ministeryAdminDtos.Last().Settlement = codeficatorRepository.GetById(ma.SettlementId).Result.Name;
        }

        return Result<IEnumerable<MinistryAdminGettingDto>>.Success(ministeryAdminDtos);
    }

    public async Task<Result<MinistryAdminGettingDto>> GetById(Guid id)
    {
        logger.LogDebug(
            $"Started getting ministry admin by {nameof(id)}:{id}.");
        var ministeryAdmin = await ministryAdminRepository.GetById(id);
        var ministeryAdminDto = mapper.Map<MinistryAdminGettingDto>(ministeryAdmin);
        ministeryAdminDto.Settlement = codeficatorRepository.GetById(ministeryAdmin.SettlementId).Result.Name;
        return Result<MinistryAdminGettingDto>.Success(ministeryAdminDto);
    }

    public async Task<Result<IEnumerable<MinistryAdminGettingDto>>> GetForMinistryId(int id)
    {
        logger.LogDebug(
            $"Started getting ministry admins by Ministry Id{nameof(id)}:{id}.");

        var ministeryAdmins = await ministryAdminRepository.GetForMinistryId(id);
        List<MinistryAdminGettingDto> ministeryAdminDtos = new List<MinistryAdminGettingDto>();
        foreach (MinistryAdmin ma in ministeryAdmins)
        {
            ministeryAdminDtos.Add(mapper.Map<MinistryAdminGettingDto>(ma));
            ministeryAdminDtos.Last().Settlement = codeficatorRepository.GetById(ma.SettlementId).Result.Name;
        }

        return Result<IEnumerable<MinistryAdminGettingDto>>.Success(ministeryAdminDtos);
    }
}
