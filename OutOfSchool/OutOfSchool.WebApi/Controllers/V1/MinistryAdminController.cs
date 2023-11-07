using Elastic.CommonSchema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutOfSchool.WebApi.Common;
using OutOfSchool.WebApi.Models;
using OutOfSchool.WebApi.Models.ChildAchievement;
using OutOfSchool.WebApi.Models.Ministry;

namespace OutOfSchool.WebApi.Controllers.V1;
[Route("api/[controller]")]
[ApiController]
public class MinistryAdminController : ControllerBase
{

    private readonly IMinistryAdminService service;

    public MinistryAdminController(IMinistryAdminService service)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Method for creating a new ministery admin.
    /// </summary>
    /// <param name="ministryAdminCreationRequestDto">Ministery admin entity to add.</param>
    /// <returns>The ministery admin that was created.</returns>
    [HasPermission(Permissions.SystemManagement)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MinistryAdminCreationResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> Create(MinistryAdminCreationRequestDto ministryAdminCreationRequestDto)
    {
        var newMinistryAdmin = await service.Create(ministryAdminCreationRequestDto);
        if (!newMinistryAdmin.Succeeded)
        {
            return BadRequest(newMinistryAdmin.OperationResult.Errors.ElementAt(0).Description);
        }

        return Created(
            nameof(newMinistryAdmin),
            newMinistryAdmin);
    }

    /// <summary>
    /// Delete the cministery admin from the database.
    /// </summary>
    /// <param name="id">The ministery admin id.</param>
    /// <returns>If deletion was successful, the result will be Status Code 204.</returns>
    [HasPermission(Permissions.SystemManagement)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await service.Delete(id);
        if (!result.Succeeded)
        {
            return BadRequest(result.OperationResult.Errors.ElementAt(0).Description);
        }

        return NoContent();
    }

    /// <summary>
    /// Update info about ministery admin in the database.
    /// </summary>
    /// <param name="ministryAdminUpdatingDto">Ministery admin entity to update.</param>
    /// <returns>The ministery admin that was updated.</returns>
    [HasPermission(Permissions.SystemManagement)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MinistryAdminUpdatingDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut]
    public async Task<IActionResult> Update(MinistryAdminUpdatingDto ministryAdminUpdatingDto)
    {
        var updatedMinistryAdmin = await service.Update(ministryAdminUpdatingDto);
        if (!updatedMinistryAdmin.Succeeded)
        {
            return BadRequest(updatedMinistryAdmin.OperationResult.Errors.ElementAt(0).Description);
        }

        return Ok(updatedMinistryAdmin);
    }

    /// <summary>
    /// Approve iministery admin status in the database.
    /// </summary>
    /// <param name="id">Ministery admin id.</param>
    /// <returns>If approve was successful, the result will be Status Code 200.</returns>
    [HasPermission(Permissions.SystemManagement)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("Approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        var result = await service.Approve(id);
        if (!result.Succeeded)
        {
            return BadRequest(result.OperationResult.Errors.ElementAt(0).Description);
        }

        return Ok(result);
    }

    /// <summary>
    /// Get all ministery admins.
    /// </summary>
    /// <returns>The ministery admins that was founded.</returns>
    [HasPermission(Permissions.SystemManagement)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResult<MinistryAdminGettingDto>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var ministryAdmins = await service.GetAll();
        if (!ministryAdmins.Succeeded)
        {
            return BadRequest(ministryAdmins.OperationResult.Errors.ElementAt(0).Description);
        }

        if (ministryAdmins.Value.Count() == 0)
        {
            return NoContent();
        }

        return Ok(ministryAdmins);
    }

    /// <summary>
    /// Get ministery admins by ministery id from the database.
    /// </summary>
    /// <param name="id">Ministery id to get ministery admins entities.</param>
    /// <returns>The ministery admins that was founded.</returns>
    [HasPermission(Permissions.SystemManagement)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResult<MinistryAdminGettingDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("GetForMinistrypId")]
    public async Task<IActionResult> GetForMinistrypId(int id)
    {
        var ministryAdmins = await service.GetForMinistryId(id);
        if (!ministryAdmins.Succeeded)
        {
            return BadRequest(ministryAdmins.OperationResult.Errors.ElementAt(0).Description);
        }

        if (ministryAdmins.Value.Count() == 0)
        {
            return NoContent();
        }

        return Ok(ministryAdmins);
    }

    /// <summary>
    /// Get ministery admins by id from the database.
    /// </summary>
    /// <param name="id">Id to get ministery admins entities.</param>
    /// <returns>The ministery admins that was founded.</returns>
    [HasPermission(Permissions.SystemManagement)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResult<MinistryAdminGettingDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("GetById")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var ministryAdmin = await service.GetById(id);
        if (!ministryAdmin.Succeeded)
        {
            return BadRequest(ministryAdmin.OperationResult.Errors.ElementAt(0).Description);
        }

        if (ministryAdmin.Value == null)
        {
            return NoContent();
        }

        return Ok(ministryAdmin);
    }
}
