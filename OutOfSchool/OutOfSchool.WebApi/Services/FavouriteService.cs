using AutoMapper;

using OutOfSchool.WebApi.Models;
using OutOfSchool.WebApi.Models.Workshops;

namespace OutOfSchool.WebApi.Services;

public class FavouriteService : IFavouriteService
{
    private readonly IEntityRepository<Guid, Favourite> favouriteRepository;
    private readonly ILogger<FavouriteService> logger;
    private readonly IMapper mapper;
    private readonly IWorkshopService workshopService;

    public FavouriteService(
        IEntityRepository<Guid, Favourite> favouriteRepository,
        ILogger<FavouriteService> logger,
        IMapper mapper,
        IWorkshopService workshopService)
    {
        this.favouriteRepository = favouriteRepository ?? throw new ArgumentNullException(nameof(favouriteRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.workshopService = workshopService ?? throw new ArgumentNullException(nameof(workshopService));
    }

    public async Task<FavouriteDto> Create(FavouriteDto dto)
    {
        _ = dto ?? throw new ArgumentNullException(nameof(dto));
        logger.LogInformation("Favourite creating was started.");

        var favourite = mapper.Map<Favourite>(dto);
        favourite.Id = default;
        favourite.WorkshopId = dto.WorkshopId;
        favourite.UserId = dto.UserId;

        var newFavourite = await favouriteRepository.Create(favourite).ConfigureAwait(false);

        logger.LogInformation($"Favourite with Id = {newFavourite.Id} created successfully.");

        return mapper.Map<FavouriteDto>(newFavourite);
    }

    public async Task Delete(Guid id)
    {
        logger.LogInformation($"Deleting Favourite with Id = {id} started.");

        var entity = await favouriteRepository.GetById(id).ConfigureAwait(false);

        try
        {
            await favouriteRepository.Delete(entity);

            logger.LogInformation($"Favourite with Id = {id} successfully deleted.");
        }
        catch (DbUpdateConcurrencyException)
        {
            logger.LogError($"Deleting Favourite with Id = {id} failed.");
            throw;
        }
    }

    public async Task<IEnumerable<WorkshopCard>> GetUserFavourites(string userId)
    {
        logger.LogInformation($"Getting Favourites by user Id - {userId} started.");

        var favouritesQuery = await favouriteRepository.GetByFilter(favourite => favourite.UserId == userId).ConfigureAwait(false);

        var favourites = favouritesQuery.ToList();

        logger.LogInformation(!favourites.Any()
            ? "Favourites table is empty."
            : $"All {favourites.Count} records were successfully received from the Favourite table for user Id - {userId}");

        var favouriteWorkshops = await workshopService.GetByIds(favourites.Select(fav => fav.WorkshopId));

        return favouriteWorkshops.Select(workshop => mapper.Map<WorkshopCard>(workshop)).ToList();
    }

    public async Task<FavouriteDto> GetById(Guid id)
    {
        logger.LogInformation($"Getting Favourite by Id started. Looking Id = {id}.");

        var favourite = await favouriteRepository.GetById(id).ConfigureAwait(false);

        if (favourite is null)
        {
            throw new ArgumentException(
                nameof(id),
                paramName: $"There are no records in favourites table with such id - {id}.");
        }

        logger.LogInformation($"Got a Favourite with Id = {id}.");

        return mapper.Map<FavouriteDto>(favourite);
    }
}
