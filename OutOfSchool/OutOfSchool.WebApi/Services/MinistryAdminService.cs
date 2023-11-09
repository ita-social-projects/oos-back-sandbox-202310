using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OutOfSchool.Common.Models;
using OutOfSchool.Services.Enums;
using OutOfSchool.Services.Models;
using OutOfSchool.Services.Repository;
using OutOfSchool.WebApi.Common;
using OutOfSchool.WebApi.Models.Ministry;
using OutOfSchool.WebApi.Services.MinistryAdminOperations;
using System.Security.Cryptography;
using System.Text;

namespace OutOfSchool.WebApi.Services;

public class MinistryAdminService : IMinistryAdminService
{
    private readonly IMinistryAdminRepository ministryAdminRepository;
    private readonly IMinistryRepository ministryRepository;
    private readonly ICodeficatorRepository codeficatorRepository;
    private readonly IMinistryAdminOperationsService ministryAdminOperationsService;
    private readonly IMapper mapper;
    private readonly ILogger<MinistryAdminService> logger;

    public MinistryAdminService(
        IMinistryAdminRepository ministryAdminRepository,
        IMinistryRepository ministryRepository,
        ICodeficatorRepository codeficatorRepository,
        IMinistryAdminOperationsService ministryAdminOperationsService,
        IMapper mapper,
        ILogger<MinistryAdminService> logger)
    {
        this.ministryAdminRepository = ministryAdminRepository;
        this.ministryRepository = ministryRepository;
        this.codeficatorRepository = codeficatorRepository;
        this.ministryAdminOperationsService = ministryAdminOperationsService;
        this.mapper = mapper;
        this.logger = logger;
    }

    public async Task<Either<ErrorResponse, CreateMinistryAdminDto>> CreateMinistryAdminAsync(
        string userId,
        CreateMinistryAdminDto ministryAdminDto,
        string token)
    {
        logger.LogDebug(
            $"Started creation of a new ministry admin {nameof(ministryAdminDto)}:{ministryAdminDto}.");
        _ = ministryAdminDto ?? throw new ArgumentNullException(nameof(ministryAdminDto));

        return await ministryAdminOperationsService
            .CreateMinistryAdminAsync(userId, ministryAdminDto, token)
            .ConfigureAwait(false);
    }

    public async Task<Either<ErrorResponse, ActionResult>> DeleteMinistryAdminAsync(Guid ministryAdminId, string userId, string token)
    {
        logger.LogDebug("MinistryAdmin(id): {ministryAdminId} deleting was started. User(id): {UserId}", ministryAdminId, userId);

        return await ministryAdminOperationsService
            .DeleteMinistryAdminAsync(ministryAdminId, userId, token)
            .ConfigureAwait(false);
    }

    public async Task<Either<ErrorResponse, UpdateMinistryAdminDto>> UpdateMinistryAdminAsync(UpdateMinistryAdminDto updateMinistryAdminDto, string userId, string token)
    {
        logger.LogDebug("MinistryAdmin(id): {ministryAdminId} updating was started. User(id): {UserId}", updateMinistryAdminDto.Id, userId);

        return await ministryAdminOperationsService
            .UpdateMinistryAdminAsync(updateMinistryAdminDto, userId, token)
            .ConfigureAwait(false);
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
        if (ministeryAdmin is null)
        {
            return Result<MinistryAdminGettingDto>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to get ministry admin " +
                $"{nameof(id)}:{id} " +
                $"was not found.",
            });
        }
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
