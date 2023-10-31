using OutOfSchool.WebApi.Models;
using OutOfSchool.WebApi.Models.Workshops;

namespace OutOfSchool.WebApi.Services;

public interface IFavouriteService
{
    /// <summary>
    /// Add entity to the database.
    /// </summary>
    /// <param name="dto">Entity to add.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="FavouriteDto"/>.</returns>
    Task<FavouriteDto> Create(FavouriteDto dto);

    /// <summary>
    /// Get entity by it's key.
    /// </summary>
    /// <param name="id">Key in the table.</param>
    /// <returns>A <see cref="Task{TEntity}"/> representing the result of the asynchronous operation.
    /// The task result contains the entity that was found, or null.</returns>
    Task<FavouriteDto> GetById(Guid id);

    /// <summary>
    /// Get all entities from the database.
    /// </summary>
    /// <param name="userId">Key in the table.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.
    /// The task result contains the <see cref="IEnumerable{TEntity}"/> that contains found elements.</returns>
    Task<IEnumerable<WorkshopCard>> GetUserFavourites(string userId);

    /// <summary>
    /// Get all entities from the database.
    /// </summary>
    /// <param name="userId">Key in the table.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.
    /// The task result contains the <see cref="IEnumerable{TEntity}"/> that contains found elements.</returns>
    Task<IEnumerable<WorkshopCard>> GetAllByUser(string userId);

    /// <summary>
    ///  Delete entity.
    /// </summary>
    /// <param name="id">Key in the table.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    Task Delete(Guid id);


}
