using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OutOfSchool.Common.Models;
using OutOfSchool.WebApi.Common;
using OutOfSchool.WebApi.Models;
using OutOfSchool.WebApi.Models.Ministry;

namespace OutOfSchool.WebApi.Controllers.V1;
[Route("api/[controller]")]
[ApiController]
public class MinistryAdminController : Controller
{

    private readonly IMinistryAdminService service;
    private string path;
    private string userId;

    public MinistryAdminController(IMinistryAdminService service)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        path = $"{context.HttpContext.Request.Path.Value}[{context.HttpContext.Request.Method}]";
        userId = GettingUserProperties.GetUserId(User);
    }

    /// <summary>
    /// Method for creating a new ministery admin.
    /// </summary>
    /// <param name="ministryAdminDto">Ministery admin entity to add.</param>
    /// <returns>The ministery admin that was created.</returns>
    [HasPermission(Permissions.SystemManagement)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SearchResult<CreateMinistryAdminDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> Create(CreateMinistryAdminDto ministryAdminDto)
    {
        var response = await service.CreateMinistryAdminAsync(
                userId,
                ministryAdminDto,
                await HttpContext.GetTokenAsync("access_token").ConfigureAwait(false))
            .ConfigureAwait(false);

        return response.Match<ActionResult>(
            error => StatusCode((int)error.HttpStatusCode, error.Message),
            result =>
            {
                return Ok(result);
            });
    }

    /// <summary>
    /// Delete the ministery admin from the database.
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
        var response = await service.DeleteMinistryAdminAsync(
                id,
                userId,
                await HttpContext.GetTokenAsync("access_token").ConfigureAwait(false))
            .ConfigureAwait(false);

        return response.Match(
            error => StatusCode((int)error.HttpStatusCode),
            _ =>
            {
                return NoContent();
            });
    }

    /// <summary>
    /// Update info about ministery admin in the database.
    /// </summary>
    /// <param name="ministryAdminDto">Ministery admin entity to update.</param>
    /// <returns>The ministery admin that was updated.</returns>
    [HasPermission(Permissions.SystemManagement)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResult<UpdateMinistryAdminDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateMinistryAdminDto ministryAdminDto)
    {
        var response = await service.UpdateMinistryAdminAsync(
                ministryAdminDto,
                userId,
                await HttpContext.GetTokenAsync("access_token").ConfigureAwait(false))
            .ConfigureAwait(false);

        return response.Match<ActionResult>(
            error => StatusCode((int)error.HttpStatusCode, error.Message),
            result =>
            {
                return Ok(result);
            });
    }

    /// <summary>
    /// Block the ministery admin from the database.
    /// </summary>
    /// <param name="id">The ministery admin id.</param>
    /// <returns>If blocking was successful, the result will be Status Code 200.</returns>
    [HasPermission(Permissions.SystemManagement)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("Block")]
    public async Task<ActionResult> Block(Guid id)
    {

        var response = await service.BlockMinistryAdminAsync(
                id,
                userId,
                await HttpContext.GetTokenAsync("access_token").ConfigureAwait(false))
            .ConfigureAwait(false);

        return response.Match(
            error => StatusCode((int)error.HttpStatusCode),
            _ =>
            {
                return Ok();
            });
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
