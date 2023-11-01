using Microsoft.AspNetCore.Mvc;

using OutOfSchool.WebApi.Common;
using OutOfSchool.WebApi.Models;
using OutOfSchool.WebApi.Models.Workshops;

namespace OutOfSchool.WebApi.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class FavouriteController : Controller
{
    private readonly IFavouriteService favouriteService;

    public FavouriteController(IFavouriteService favouriteService)
    {
        this.favouriteService = favouriteService;
    }

    [HasPermission(Permissions.FavoriteRead)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WorkshopCard>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        string userId = GettingUserProperties.GetUserId(User);

        var favourites = await favouriteService.GetUserFavourites(userId).ConfigureAwait(false);

        if (!favourites.Any())
        {
            return NoContent();
        }

        return Ok(favourites);
    }

    [HasPermission(Permissions.FavoriteAddNew)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> Create(FavouriteDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var creationResult = await favouriteService.Create(dto).ConfigureAwait(false);

        return CreatedAtAction(
            nameof(GetById),
            new { id = creationResult.Id },
            creationResult);
    }

    [HasPermission(Permissions.FavoriteRead)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FavouriteDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var favouriteDto = await favouriteService.GetById(id).ConfigureAwait(false);
            return Ok(favouriteDto);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HasPermission(Permissions.FavoriteRemove)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await favouriteService.Delete(id).ConfigureAwait(false);

        return NoContent();
    }
}
