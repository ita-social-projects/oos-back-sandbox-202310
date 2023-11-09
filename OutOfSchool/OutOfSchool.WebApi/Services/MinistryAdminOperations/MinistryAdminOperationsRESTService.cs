using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OutOfSchool.Common.Models;
using OutOfSchool.Services.Repository;
using OutOfSchool.WebApi.Models;

namespace OutOfSchool.WebApi.Services.MinistryAdminOperations;

public class MinistryAdminOperationsRESTService : CommunicationService, IMinistryAdminOperationsService
{
    private readonly AuthorizationServerConfig authorizationServerConfig;
    private readonly IMinistryAdminRepository ministryAdminRepository;

    public MinistryAdminOperationsRESTService(
        IMinistryAdminRepository ministryAdminRepository,
        ILogger<ProviderAdminOperationsRESTService> logger,
        IOptions<AuthorizationServerConfig> authorizationServerConfig,
        IHttpClientFactory httpClientFactory,
        IOptions<CommunicationConfig> communicationConfig)
        : base(httpClientFactory, communicationConfig.Value, logger)
    {
        this.authorizationServerConfig = authorizationServerConfig.Value;
        this.ministryAdminRepository = ministryAdminRepository;
    }

    public async Task<Either<ErrorResponse, CreateMinistryAdminDto>> CreateMinistryAdminAsync(string userId, CreateMinistryAdminDto ministryAdminDto, string token)
    {
        var request = new Request()
        {
            HttpMethodType = HttpMethodType.Post,
            Url = new Uri(authorizationServerConfig.Authority, CommunicationConstants.CreateMinistryAdmin),
            Token = token,
            Data = ministryAdminDto,
        };

        Logger.LogDebug(
            "{HttpMethodType} Request was sent. User(id): {UserId}. Url: {Url}",
            request.HttpMethodType,
            userId,
            request.Url);

        var response = await SendRequest<ResponseDto>(request)
            .ConfigureAwait(false);

        return response
            .FlatMap<ResponseDto>(r => r.IsSuccess
                ? r
                : new ErrorResponse
                {
                    HttpStatusCode = r.HttpStatusCode,
                    Message = r.Message,
                })
            .Map(result => result.Result is not null
                ? JsonConvert
                    .DeserializeObject<CreateMinistryAdminDto>(result.Result.ToString())
                : null);
    }

    public async Task<Either<ErrorResponse, ActionResult>> DeleteMinistryAdminAsync(Guid ministryAdminId, string userId, string token)
    {
        var minisrtyAdmin = await ministryAdminRepository.GetById(ministryAdminId)
            .ConfigureAwait(false);

        if (minisrtyAdmin is null)
        {
            Logger.LogError("minisrtyAdmin(id) {ministryAdminId} not found. User(id): {UserId}", ministryAdminId, userId);

            return new ErrorResponse
            {
                HttpStatusCode = HttpStatusCode.NotFound,
            };
        }

        var request = new Request()
        {
            HttpMethodType = HttpMethodType.Delete,
            Url = new Uri(authorizationServerConfig.Authority, CommunicationConstants.DeleteMinistryAdmin + ministryAdminId),
            Token = token,
        };

        Logger.LogDebug(
            "{HttpMethodType} Request was sent. User(id): {UserId}. Url: {Url}",
            request.HttpMethodType,
            userId,
            request.Url);

        var response = await SendRequest<ResponseDto>(request)
            .ConfigureAwait(false);

        return response
            .FlatMap<ResponseDto>(r => r.IsSuccess
                ? r
                : new ErrorResponse
                {
                    HttpStatusCode = r.HttpStatusCode,
                    Message = r.Message,
                })
            .Map(result => result.Result is not null
                ? JsonConvert
                    .DeserializeObject<ActionResult>(result.Result.ToString())
                : null);
    }
}
