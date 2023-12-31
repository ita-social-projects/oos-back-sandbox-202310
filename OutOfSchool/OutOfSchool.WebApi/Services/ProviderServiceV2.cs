using AutoMapper;
using Microsoft.Extensions.Localization;
using OutOfSchool.Services.Models.Images;
using OutOfSchool.WebApi.Models.Providers;

namespace OutOfSchool.WebApi.Services;

public class ProviderServiceV2 : ProviderService, IProviderServiceV2
{
    public ProviderServiceV2(
        IProviderRepository providerRepository,
        IEntityRepositorySoftDeleted<string, User> usersRepository,
        ILogger<ProviderServiceV2> logger,
        IStringLocalizer<SharedResource> localizer,
        IMapper mapper,
        IEntityRepositorySoftDeleted<long, Address> addressRepository,
        IWorkshopServicesCombiner workshopServiceCombiner,
        IProviderAdminRepository providerAdminRepository,
        IImageDependentEntityImagesInteractionService<Provider> providerImagesService,
        IChangesLogService changesLogService,
        INotificationService notificationService,
        IProviderAdminService providerAdminService,
        ICurrentUserService currentUserService,
        ICodeficatorService codeficatorService)
        : base(
              providerRepository,
              usersRepository,
              logger,
              localizer,
              mapper,
              addressRepository,
              workshopServiceCombiner,
              providerAdminRepository,
              providerImagesService,
              changesLogService,
              notificationService,
              providerAdminService,
              currentUserService,
              codeficatorService)
    {
    }

    /// <inheritdoc cref="IProviderServiceV2" />
    public new async Task<ProviderDto> Create(ProviderDto providerDto)
    {
        async Task AfterCreationAction(Provider provider)
        {
            if (providerDto.ImageFiles?.Count > 0)
            {
                provider.Images = new List<Image<Provider>>();
                await ProviderImagesService.AddManyImagesAsync(provider, providerDto.ImageFiles)
                    .ConfigureAwait(false);
            }

            if (providerDto.CoverImage != null)
            {
                await ProviderImagesService.AddCoverImageAsync(provider, providerDto.CoverImage)
                    .ConfigureAwait(false);
            }
        }

        return await CreateProviderWithActionAfterAsync(providerDto, AfterCreationAction).ConfigureAwait(false);
    }

    public new async Task<ProviderDto> Update(ProviderUpdateDto providerDto, string userId)
    {
        async Task BeforeUpdateAction(Provider provider)
        {
            await ProviderImagesService.ChangeImagesAsync(provider, providerDto.ImageIds ?? new List<string>(), providerDto.ImageFiles)
                .ConfigureAwait(false);

            await ProviderImagesService.ChangeCoverImageAsync(provider, providerDto.CoverImageId, providerDto.CoverImage).ConfigureAwait(false);
        }

        return await UpdateProviderWithActionBeforeSavingChanges(providerDto, userId, BeforeUpdateAction)
            .ConfigureAwait(false);
    }

    public new async Task Delete(Guid id)
    {
        async Task BeforeDeleteAction(Provider provider)
        {
            if (provider.Images.Count > 0)
            {
                await ProviderImagesService.RemoveManyImagesAsync(provider, provider.Images.Select(x => x.ExternalStorageId).ToList()).ConfigureAwait(false);
            }

            if (!string.IsNullOrEmpty(provider.CoverImageId))
            {
                await ProviderImagesService.RemoveCoverImageAsync(provider).ConfigureAwait(false);
            }
        }

        await DeleteProviderWithActionBefore(id, BeforeDeleteAction).ConfigureAwait(false);
    }
}