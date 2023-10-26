using AutoMapper;
using Microsoft.Extensions.Options;

namespace OutOfSchool.WebApi.Services;

public class ChildAchievementService
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


}
