using AutoMapper;
using OutOfSchool.WebApi.Common;
using OutOfSchool.WebApi.Models.ChildAchievement;
using OutOfSchool.WebApi.Models.Ministry;

namespace OutOfSchool.WebApi.Services;

public class MinistryService : IMinistryService
{
    private readonly IMinistryRepository ministryRepository;
    private readonly IMapper mapper;
    private readonly ILogger<MinistryService> logger;

    public MinistryService(IMinistryRepository ministryRepository, IMapper mapper, ILogger<MinistryService> logger)
    {
        this.ministryRepository = ministryRepository;
        this.mapper = mapper;
        this.logger = logger;
    }

    public async Task<Result<MinistryCreationResponseDto>> Create(MinistryCreationRequestDto ministryCreationRequestDto)
    {
        logger.LogDebug(
            $"Started creation of a new ministry {nameof(ministryCreationRequestDto)}:{ministryCreationRequestDto}.");
        _ = ministryCreationRequestDto ?? throw new ArgumentNullException(nameof(ministryCreationRequestDto));

        var ministry = mapper.Map<Ministry>(ministryCreationRequestDto);
        return Result<MinistryCreationResponseDto>.Success(
            mapper.Map<MinistryCreationResponseDto>(await ministryRepository.Create(ministry)));
    }

    public async Task<Result<object>> Delete(int id)
    {
        logger.LogDebug(
            $"Started deleting ministry {nameof(id)}:{id}.");

        var min = await ministryRepository.GetById(id).ConfigureAwait(false);
        if (min is null)
        {
            return Result<object>.Failed(new OperationError
            {
                Code = "400",
                Description = $"Trying to delete not existing Ministry (Id = {id}).",
            });
        }

        await ministryRepository.DeleteById(id);

        logger.LogDebug(
            $"Ministry deleted{nameof(id)}:{id}.");
        return Result<object>.Success(null);
    }

    public async Task<Result<IEnumerable<MinistryGettingDto>>> GetAll()
    {
        logger.LogDebug(
            $"Started creation of getting all ministries.");
        var ministeries = await ministryRepository.GetAll();
        List<MinistryGettingDto> ministeriesDto = new List<MinistryGettingDto>();
        foreach (Ministry m in ministeries) {
            ministeriesDto.Add(mapper.Map<MinistryGettingDto>(m));
        }

        return Result<IEnumerable<MinistryGettingDto>>.Success(ministeriesDto);
    }

    public async Task<Result<MinistryGettingDto>> GetById(int id)
    {
        logger.LogDebug(
            $"Started getting ministry by id {nameof(id)}:{id}.");
        return Result<MinistryGettingDto>.Success(mapper.Map<MinistryGettingDto>(await ministryRepository.GetById(id)));
    }
}
