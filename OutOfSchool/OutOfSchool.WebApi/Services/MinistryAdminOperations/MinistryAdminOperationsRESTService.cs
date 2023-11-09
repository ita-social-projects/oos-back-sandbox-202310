using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OutOfSchool.Common.Models;
using OutOfSchool.WebApi.Models;

namespace OutOfSchool.WebApi.Services.MinistryAdminOperations;

public class MinistryAdminOperationsRESTService : CommunicationService, IMinistryAdminOperationsService
{
    private readonly AuthorizationServerConfig authorizationServerConfig;

    public MinistryAdminOperationsRESTService(
        ILogger<ProviderAdminOperationsRESTService> logger,
        IOptions<AuthorizationServerConfig> authorizationServerConfig,
        IHttpClientFactory httpClientFactory,
        IOptions<CommunicationConfig> communicationConfig)
        : base(httpClientFactory, communicationConfig.Value, logger)
    {
        this.authorizationServerConfig = authorizationServerConfig.Value;
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
}
