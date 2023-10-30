using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutOfSchool.WebApi.Common;
using OutOfSchool.WebApi.Models;
using OutOfSchool.WebApi.Models.ChildAchievement;

namespace OutOfSchool.WebApi.Controllers.V1;
[Route("api/[controller]")]
[ApiController]
public class ChildAchievementController : ControllerBase
{
    private readonly IChildAchievementService service;

    public ChildAchievementController(IChildAchievementService service)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Method for creating a new child achievement.
    /// </summary>
    /// <param name="childAchievementCreationRequestDto">Child achievement entity to add.</param>
    /// <returns>The child achievement that was created.</returns>
    [HasPermission(Permissions.ChildAchievementCreate)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ChildAchievementCreationResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> Create(ChildAchievementCreationRequestDto childAchievementCreationRequestDto) {
        string userId = GettingUserProperties.GetUserId(User);
        var newAchive = await service.CreateAchievement(childAchievementCreationRequestDto, userId);
        return Created(
            nameof(newAchive),
            newAchive);
    }

    /// <summary>
    /// Delete the child achievement from the database.
    /// </summary>
    /// <param name="id">The child achievement id.</param>
    /// <returns>If deletion was successful, the result will be Status Code 204.</returns>
    [HasPermission(Permissions.ChildAchievementDelete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        string userId = GettingUserProperties.GetUserId(User);
        await service.DeleteAchievement(id, userId);
        return NoContent();
    }

    /// <summary>
    /// Update info about child achievement in the database.
    /// </summary>
    /// <param name="childAchievementUpdatingRequestDto">Child achievement entity to update.</param>
    /// <returns>The child achievement that was updated.</returns>
    [HasPermission(Permissions.ChildAchievementUpdate)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChildAchievementUpdatingResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut]
    public async Task<IActionResult> Update(ChildAchievementUpdatingRequestDto childAchievementUpdatingRequestDto)
    {
        string userId = GettingUserProperties.GetUserId(User);
        var updatedAchive = await service.UpdateAchievement(childAchievementUpdatingRequestDto, userId);
        return Ok(updatedAchive);
    }

    /// <summary>
    /// Get children achievements by child id from the database.
    /// </summary>
    /// <param name="id">Child id to get achievement entity.</param>
    /// <returns>The child achievement that was founded.</returns>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResult<ChildAchievementGettingDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("GetForChildId")]
    public async Task<IActionResult> GetForChildId(Guid id)
    {
        var childAchievements = await service.GetAchievementForChildId(id);
        if (childAchievements.Count() == 0)
        {
            return NoContent();
        }

        return Ok(childAchievements);
    }

    /// <summary>
    /// Get children achievements by workshop id from the database.
    /// </summary>
    /// <param name="id">Workshop id to get achievement entity.</param>
    /// <returns>The child achievement that was founded.</returns>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResult<ChildAchievementGettingDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("GetForWorkshopId")]
    public async Task<IActionResult> GetForWorkshopId(Guid id)
    {
        var childAchievements = await service.GetAchievementForWorkshopId(id);
        if (childAchievements.Count() == 0)
        {
            return NoContent();
        }

        return Ok(childAchievements);
    }

    /// <summary>
    /// Get children achievements by child and workshop id's from the database.
    /// </summary>
    /// <param name="childId">Child id to get achievement entity.</param>
    /// <param name="workshopId">Workshop id to get achievement entity.</param>
    /// <returns>The child achievement that was founded.</returns>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResult<ChildAchievementGettingDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("GetForChildIdWorkshopId")]
    public async Task<IActionResult> GetForChildIdWorkshopId(Guid childId,Guid workshopId)
    {
        var childAchievements = await service.GetAchievementForWorkshopIdChildId(childId, workshopId);

        if (childAchievements.Count() == 0)
        {
            return NoContent();
        }

        return Ok(childAchievements);
    }

    /// <summary>
    /// Get all children achievements.
    /// </summary>
    /// <returns>The child achievements that was founded.</returns>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResult<ChildAchievementGettingDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var childAchievements = await service.GetAll();

        if (childAchievements.Count() == 0)
        {
            return NoContent();
        }

        return Ok(childAchievements);
    }
}
