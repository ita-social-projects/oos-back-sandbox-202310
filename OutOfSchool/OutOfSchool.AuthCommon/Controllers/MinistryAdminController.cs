using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OutOfSchool.Common.Models;

namespace OutOfSchool.AuthCommon.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class MinistryAdminController : Controller
{
    private readonly ILogger<ProviderAdminController> logger;
    private readonly IMinistryAdminService ministryAdminService;

    private string userId;

    public MinistryAdminController(
        ILogger<ProviderAdminController> logger,
        IMinistryAdminService ministryAdminService)
    {
        this.logger = logger;
        this.ministryAdminService = ministryAdminService;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        userId = User.GetUserPropertyByClaimType(IdentityResourceClaimsTypes.Sub);
    }

    [HttpPost]
    public async Task<ResponseDto> Create(CreateMinistryAdminDto ministryAdminDto)
    {
        logger.LogDebug("Operation initiated by User(id): {UserId}", userId);

        return await ministryAdminService
            .CreateMinistryAdminAsync(ministryAdminDto, Url, userId);
    }

    [HttpDelete("{ministryAdminId}")]
    public async Task<ResponseDto> Delete(Guid ministryAdminId)
    {
        logger.LogDebug("Operation initiated by User(id): {UserId}", userId);

        return await ministryAdminService
            .DeleteMinistryAdminAsync(ministryAdminId, userId);
    }

    [HttpPut("{ministryAdminId}")]
    public async Task<ResponseDto> Update(Guid ministryAdminId, UpdateMinistryAdminDto ministryAdminDto)
    {
        logger.LogDebug(
            "Operation initiated by User(id): {UserId}",
            userId);

        return await ministryAdminService.
            UpdateMinistryAdminAsync(ministryAdminDto, userId);
    }

    [HttpPut("{ministryAdminId}")]
    public async Task<ResponseDto> Block(Guid ministryAdminId)
    {
        logger.LogDebug("Operation initiated by User(id): {UserId}", userId);

        return await ministryAdminService
            .BlockMinistryAdminAsync(ministryAdminId, userId);
    }
}
