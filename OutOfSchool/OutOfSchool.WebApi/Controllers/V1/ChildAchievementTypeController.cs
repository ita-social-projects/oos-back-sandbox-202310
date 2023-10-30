using Elastic.CommonSchema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutOfSchool.WebApi.Common;
using OutOfSchool.WebApi.Models;
using OutOfSchool.WebApi.Models.ChildAchievement;

namespace OutOfSchool.WebApi.Controllers.V1;
[Route("api/[controller]")]
[ApiController]
public class ChildAchievementTypeController : ControllerBase
{
    private readonly IChildAchievementTypeService service;

    public ChildAchievementTypeController(IChildAchievementTypeService childAchievementTypeService)
    {
        this.service = childAchievementTypeService ?? throw new ArgumentNullException(nameof(childAchievementTypeService));
    }

    /// <summary>
    /// Method for creating a new child achievement type.
    /// </summary>
    /// <param name="childAchievementTypeRequestDto">Child achievement type entity to add. Localization ('Ua','En').</param>
    /// <returns>The child achievement type that was created.</returns>
    [HasPermission(Permissions.SystemManagement)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ChildAchievementType))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> Create(ChildAchievementTypeRequestDto childAchievementTypeRequestDto)
    {
        var newAchiveType = await service.CreateAchievementType(childAchievementTypeRequestDto);
        return Created(
            nameof(newAchiveType),
            newAchiveType);
    }

    /// <summary>
    /// Delete the child achievement type from the database.
    /// </summary>
    /// <param name="id">The child achievement type id.</param>
    /// <returns>If deletion was successful, the result will be Status Code 204.</returns>
    [HasPermission(Permissions.SystemManagement)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAchievementType(id);
        return NoContent();
    }

    /// <summary>
    /// Get children achievement type by id from the database.
    /// </summary>
    /// <param name="id">Achievement type id to get achievement type entity.</param>
    /// <returns>The child achievement type that was founded.</returns>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChildAchievementType))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("GetById")]
    public async Task<IActionResult> GetById(int id)
    {
        var childAchievementType = await service.GetAchievementTypeById(id);
        if (childAchievementType == null)
        {
            return NoContent();
        }

        return Ok(childAchievementType);
    }

    /// <summary>
    /// Get all children achievement types.
    /// </summary>
    /// <returns>The child achievement types that was founded.</returns>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResult<ChildAchievementType>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var childAchievementTypes = await service.GetAllAchievementTypes();

        if (childAchievementTypes.Count() == 0)
        {
            return NoContent();
        }

        return Ok(childAchievementTypes);
    }
}
