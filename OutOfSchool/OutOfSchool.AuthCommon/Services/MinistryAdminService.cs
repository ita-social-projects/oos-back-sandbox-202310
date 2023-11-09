using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OutOfSchool.AuthCommon.Config;
using OutOfSchool.AuthCommon.Config.ExternalUriModels;
using OutOfSchool.AuthCommon.Extensions;
using OutOfSchool.Common.Enums;
using OutOfSchool.Common.Models;
using OutOfSchool.RazorTemplatesData.Models.Emails;
using OutOfSchool.Services.Enums;

namespace OutOfSchool.AuthCommon.Services;
public class MinistryAdminService : IMinistryAdminService
{
    private readonly IEmailSender emailSender;
    private readonly IMapper mapper;
    private readonly ILogger<ProviderAdminService> logger;
    private readonly IMinistryAdminRepository ministryAdminRepository;
    private readonly IMinistryRepository ministryRepository;
    private readonly ICodeficatorRepository codeficatorRepository;
    private readonly AngularClientScopeExternalUrisConfig externalUrisConfig;
    private readonly ChangesLogConfig changesLogConfig;

    private readonly UserManager<User> userManager;
    private readonly OutOfSchoolDbContext context;
    private readonly IRazorViewToStringRenderer renderer;

    public MinistryAdminService(
        IMapper mapper,
        IMinistryAdminRepository ministryAdminRepository,
        IMinistryRepository ministryRepository,
        ICodeficatorRepository codeficatorRepository,
        ILogger<ProviderAdminService> logger,
        IEmailSender emailSender,
        UserManager<User> userManager,
        OutOfSchoolDbContext context,
        IRazorViewToStringRenderer renderer,
        IOptions<AngularClientScopeExternalUrisConfig> externalUrisConfig,
        IOptions<ChangesLogConfig> changesLogConfig)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.ministryAdminRepository = ministryAdminRepository ?? throw new ArgumentNullException(nameof(ministryAdminRepository));
        this.ministryRepository = ministryRepository ?? throw new ArgumentNullException(nameof(ministryRepository));
        this.codeficatorRepository = codeficatorRepository ?? throw new ArgumentNullException(nameof(codeficatorRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        this.externalUrisConfig =
            externalUrisConfig?.Value ?? throw new ArgumentNullException(nameof(externalUrisConfig));
        this.changesLogConfig = changesLogConfig?.Value ?? throw new ArgumentNullException(nameof(changesLogConfig));
    }

    public async Task<ResponseDto> BlockMinistryAdminAsync(Guid ministryAdminId, string userId)
    {
        var response = new ResponseDto();

        var ministryAdmin = ministryAdminRepository.GetById(ministryAdminId).Result;

        if (ministryAdmin is null)
        {
            response.IsSuccess = false;
            response.HttpStatusCode = HttpStatusCode.NotFound;

            logger.LogError(
                "ministryAdmin(id) {ministryAdminId} not found. User(id): {UserId}",
                ministryAdminId,
                userId);

            return response;
        }

        var user = await userManager.FindByIdAsync(ministryAdminId.ToString());

        var executionStrategy = context.Database.CreateExecutionStrategy();
        return await executionStrategy.Execute(BlockMinistryAdminOperation).ConfigureAwait(false);

        async Task<ResponseDto> BlockMinistryAdminOperation()
        {
            await using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);
            try
            {
                user.IsBlocked = true;
                var updateResult = await userManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                {
                    await transaction.RollbackAsync().ConfigureAwait(false);

                    logger.LogError(
                        "Error happened while blocking ministryAdmin. User(id): {UserId}. {Errors}",
                        userId,
                        string.Join(Environment.NewLine, updateResult.Errors.Select(e => e.Description)));

                    response.IsSuccess = false;
                    response.HttpStatusCode = HttpStatusCode.InternalServerError;

                    return response;
                }

                var updateSecurityStamp = await userManager.UpdateSecurityStampAsync(user);

                if (!updateSecurityStamp.Succeeded)
                {
                    await transaction.RollbackAsync().ConfigureAwait(false);

                    logger.LogError(
                        "Error happened while updating security stamp. ministryAdmin. User(id): {UserId}. {Errors}",
                        userId,
                        string.Join(Environment.NewLine, updateSecurityStamp.Errors.Select(e => e.Description)));

                    response.IsSuccess = false;
                    response.HttpStatusCode = HttpStatusCode.InternalServerError;

                    return response;
                }

                await transaction.CommitAsync().ConfigureAwait(false);
                response.IsSuccess = true;
                response.HttpStatusCode = HttpStatusCode.OK;

                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync().ConfigureAwait(false);

                logger.LogError(
                    ex,
                    "Error happened while blocking ministryAdmin. User(id): {UserId}",
                    userId);

                response.IsSuccess = false;
                response.HttpStatusCode = HttpStatusCode.InternalServerError;

                return response;
            }
        }
    }

    public async Task<ResponseDto> CreateMinistryAdminAsync(CreateMinistryAdminDto ministryAdminDto, IUrlHelper url, string userId)
    {

        var user = mapper.Map<User>(ministryAdminDto);

        var password = ministryAdminDto.Password;

        var executionStrategy = context.Database.CreateExecutionStrategy();
        var result = await executionStrategy.Execute(async () =>
        {
            var response = new ResponseDto();

            if (await ministryRepository.GetById(ministryAdminDto.MinistryId) is null)
            {
                response.IsSuccess = false;
                response.HttpStatusCode = HttpStatusCode.BadRequest;
                response.Message = $"Trying to create a new ministry admin the Ministry with " +
                    $"{nameof(ministryAdminDto.MinistryId)}:{ministryAdminDto.MinistryId} " +
                    $"was not found.";

                return response;
            }

            var settlement = await codeficatorRepository.GetById(ministryAdminDto.SettlementId);
            if (settlement is null)
            {
                response.IsSuccess = false;
                response.HttpStatusCode = HttpStatusCode.BadRequest;
                response.Message = $"Trying to create a new ministry admin the Settlement with " +
                    $"{nameof(ministryAdminDto.SettlementId)}:{ministryAdminDto.SettlementId} " +
                    $"was not found.";

                return response;
            }

            if (settlement.Category == CodeficatorCategory.Region.Name ||
            settlement.Category == CodeficatorCategory.District.Name ||
            settlement.Category == CodeficatorCategory.CityDistrict.Name)
            {
                response.IsSuccess = false;
                response.HttpStatusCode = HttpStatusCode.BadRequest;
                response.Message = $"Trying to create a new ministry admin the Settlement with " +
                    $"{nameof(ministryAdminDto.SettlementId)}:{ministryAdminDto.SettlementId} " +
                    $"not confirmed.";

                return response;
            }

            if (await context.Users.AnyAsync(x => x.Email == ministryAdminDto.Email).ConfigureAwait(false))
            {
                logger.LogError("Cant create ministry admin with duplicate email: {Email}", ministryAdminDto.Email);
                response.IsSuccess = false;
                response.HttpStatusCode = HttpStatusCode.BadRequest;
                response.Message = $"Cant create ministry admin with duplicate email: {ministryAdminDto.Email}";

                return response;
            }

            await using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);
            try
            {
                user.IsDerived = true;
                user.IsRegistered = true;
                user.IsBlocked = false;
                user.Role = nameof(Role.MinistryAdmin).ToLower();
                var result = await userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();

                    logger.LogError(
                        "Error happened while creation ministryAdmin. User(id): {UserId}. {Errors}",
                        userId,
                        result.ErrorMessages());

                    response.IsSuccess = false;
                    response.HttpStatusCode = HttpStatusCode.BadRequest;

                    response.Message = string.Join(
                        Environment.NewLine,
                        result.Errors.Select(e => e.Description));

                    return response;
                }

                var roleAssignResult = await userManager.AddToRoleAsync(user, user.Role);

                if (!roleAssignResult.Succeeded)
                {
                    await transaction.RollbackAsync();

                    logger.LogError(
                        "Error happened while adding role to user. User(id): {UserId}. {Errors}",
                        userId,
                        result.ErrorMessages());

                    response.IsSuccess = false;
                    response.HttpStatusCode = HttpStatusCode.InternalServerError;

                    return response;
                }

                ministryAdminDto.UserId = user.Id;

                var ministryAdmin = mapper.Map<MinistryAdmin>(ministryAdminDto);
                ministryAdmin.Id = Guid.Parse(user.Id);

                await ministryAdminRepository.Create(ministryAdmin)
                    .ConfigureAwait(false);

                logger.LogInformation(
                    "ministryAdmin(id):{Id} was successfully created by User(id): {UserId}",
                    ministryAdminDto.UserId,
                    userId);

                await this.SendInvitationEmail(user, url, password);

                await transaction.CommitAsync();
                response.IsSuccess = true;
                response.HttpStatusCode = HttpStatusCode.OK;
                response.Result = ministryAdminDto;

                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                logger.LogError(ex, "Operation failed for User(id): {UserId}", userId);

                response.IsSuccess = false;
                response.HttpStatusCode = HttpStatusCode.InternalServerError;

                return response;
            }
        });
        return result;
    }

    public async Task<ResponseDto> DeleteMinistryAdminAsync(Guid ministryAdminId, string userId)
    {
        var executionStrategy = context.Database.CreateExecutionStrategy();
        var result = await executionStrategy.Execute(async () =>
        {
            var response = new ResponseDto();
            await using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);
            try
            {
                var ministryAdmin = ministryAdminRepository.GetById(ministryAdminId).Result;

                if (ministryAdmin is null)
                {
                    response.IsSuccess = false;
                    response.HttpStatusCode = HttpStatusCode.NotFound;

                    logger.LogError(
                        "ministryAdmin(id) {ministryAdminId} not found. User(id): {UserId}",
                        ministryAdminId,
                        userId);

                    return response;
                }

                context.MinistryAdmins.Remove(ministryAdmin);

                var user = await userManager.FindByIdAsync((ministryAdmin.Id).ToString());
                var result = await userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();

                    logger.LogError(
                        "Error happened while deleting ministryAdmin. User(id): {UserId}. {Errors}",
                        userId,
                        result.ErrorMessages());

                    response.IsSuccess = false;
                    response.HttpStatusCode = HttpStatusCode.InternalServerError;

                    return response;
                }

                await transaction.CommitAsync();
                response.IsSuccess = true;
                response.HttpStatusCode = HttpStatusCode.OK;

                logger.LogInformation(
                    "ministryAdmin(id):{ministryAdminId} was successfully deleted by User(id): {UserId}",
                    ministryAdminId,
                    userId);

                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                logger.LogError(
                    ex,
                    "Error happened while deleting MinistryAdmin. User(id): {UserId}",
                    userId);

                response.IsSuccess = false;
                response.HttpStatusCode = HttpStatusCode.InternalServerError;

                return response;
            }
        });
        return result;
    }

    public async Task<ResponseDto> UpdateMinistryAdminAsync(UpdateMinistryAdminDto ministryAdminDto, string userId)
    {
        _ = ministryAdminDto ?? throw new ArgumentNullException(nameof(ministryAdminDto));

        var response = new ResponseDto();

        if (await context.Users.AnyAsync(x => x.Email == ministryAdminDto.Email
            && x.Id != ministryAdminDto.Id.ToString()).ConfigureAwait(false))
        {
            logger.LogError("Cant update ministry admin with duplicate email: {Email}", ministryAdminDto.Email);
            response.IsSuccess = false;
            response.HttpStatusCode = HttpStatusCode.BadRequest;
            response.Message = $"Cant update ministry admin with duplicate email: {ministryAdminDto.Email}";

            return response;
        }

        if (await ministryRepository.GetById(ministryAdminDto.MinistryId) is null)
        {
            response.IsSuccess = false;
            response.HttpStatusCode = HttpStatusCode.BadRequest;
            response.Message = $"Trying to update ministry admin the Ministry with " +
                $"{nameof(ministryAdminDto.MinistryId)}:{ministryAdminDto.MinistryId} " +
                $"was not found.";

            return response;
        }

        var settlement = await codeficatorRepository.GetById(ministryAdminDto.SettlementId);
        if (settlement is null)
        {
            response.IsSuccess = false;
            response.HttpStatusCode = HttpStatusCode.BadRequest;
            response.Message = $"Trying to update a new ministry admin the Settlement with " +
                $"{nameof(ministryAdminDto.SettlementId)}:{ministryAdminDto.SettlementId} " +
                $"was not found.";

            return response;
        }

        if (settlement.Category == CodeficatorCategory.Region.Name ||
            settlement.Category == CodeficatorCategory.District.Name ||
            settlement.Category == CodeficatorCategory.CityDistrict.Name)
        {
            response.IsSuccess = false;
            response.HttpStatusCode = HttpStatusCode.BadRequest;
            response.Message = $"Trying to create a new ministry admin the Settlement with " +
                $"{nameof(ministryAdminDto.SettlementId)}:{ministryAdminDto.SettlementId} " +
                $"not confirmed.";

            return response;
        }

        var ministryAdmin = ministryAdminRepository.GetById(ministryAdminDto.Id).Result;

        if (ministryAdmin is null)
        {
            response.IsSuccess = false;
            response.HttpStatusCode = HttpStatusCode.NotFound;

            logger.LogError(
                "ministryAdmin(id) {Id} not found. User(id): {UserId}",
                ministryAdminDto.Id,
                userId);

            return response;
        }

        var user = await userManager.FindByIdAsync(ministryAdmin.Id.ToString());
        if (user.EmailConfirmed == true)
        {
            response.IsSuccess = false;
            response.HttpStatusCode = HttpStatusCode.BadRequest;
            response.Message = $"Trying to update a new ministry wich already logined";
        }

        var executionStrategy = context.Database.CreateExecutionStrategy();
        return await executionStrategy.Execute(ministryAdminDto, UpdateMinistryAdminOperation).ConfigureAwait(false);

        async Task<ResponseDto> UpdateMinistryAdminOperation(UpdateMinistryAdminDto updateDto)
        {
            await using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);
            try
            {
                user.FirstName = updateDto.FirstName;
                user.LastName = updateDto.LastName;
                user.MiddleName = updateDto.MiddleName;
                user.UserName = updateDto.Email;
                user.Email = updateDto.Email;
                user.PhoneNumber = Constants.PhonePrefix + updateDto.PhoneNumber;
                var updateResult = await userManager.UpdateAsync(user);


                if (!updateResult.Succeeded)
                {
                    await transaction.RollbackAsync().ConfigureAwait(false);

                    logger.LogError(
                        "Error happened while updating ministryAdmin. User(id): {UserId}. {Errors}",
                        userId,
                        string.Join(Environment.NewLine, updateResult.Errors.Select(e => e.Description)));

                    response.IsSuccess = false;
                    response.HttpStatusCode = HttpStatusCode.InternalServerError;

                    return response;
                }

                var updateSecurityStamp = await userManager.UpdateSecurityStampAsync(user);

                if (!updateSecurityStamp.Succeeded)
                {
                    await transaction.RollbackAsync().ConfigureAwait(false);

                    logger.LogError(
                        "Error happened while updating security stamp. ministryAdmin. User(id): {UserId}. {Errors}",
                        userId,
                        string.Join(Environment.NewLine, updateSecurityStamp.Errors.Select(e => e.Description)));

                    response.IsSuccess = false;
                    response.HttpStatusCode = HttpStatusCode.InternalServerError;

                    return response;
                }

                context.Entry(user).State = EntityState.Detached;
                context.Entry(ministryAdmin).State = EntityState.Detached;
                ministryAdmin = mapper.Map<MinistryAdmin>(ministryAdminDto);
                var ministryAdminUpd = await ministryAdminRepository.Update(ministryAdmin);

                await transaction.CommitAsync().ConfigureAwait(false);

                logger.LogInformation(
                    "ministryAdmin(id):{Id} was successfully updated by User(id): {UserId}",
                    updateDto.Id,
                    userId);

                response.IsSuccess = true;
                response.HttpStatusCode = HttpStatusCode.OK;
                response.Result = updateDto;
                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync().ConfigureAwait(false);

                logger.LogError(
                    ex,
                    "Error happened while updating ministryAdmin. User(id): {UserId}",
                    userId);

                response.IsSuccess = false;
                response.HttpStatusCode = HttpStatusCode.InternalServerError;

                return response;
            }
        }
    }

    private async Task SendInvitationEmail(User user, IUrlHelper url, string password)
    {
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

        string confirmationLink = url.Action(
                    "EmailConfirmation",
                    "Account",
                    new { email = user.Email, token, redirectUrl = externalUrisConfig.Login },
                    "https");

        var subject = "Запрошення!";
        var adminInvitationViewModel = new AdminInvitationViewModel
        {
            ConfirmationUrl = confirmationLink,
            Email = user.Email,
            Password = password,
        };
        var content = await renderer.GetHtmlPlainStringAsync(RazorTemplates.NewAdminInvitation, adminInvitationViewModel);

        await emailSender.SendAsync(user.Email, subject, content);
    }
}
