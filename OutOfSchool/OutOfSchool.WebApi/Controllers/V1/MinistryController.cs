using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutOfSchool.WebApi.Models;
using OutOfSchool.WebApi.Models.ChildAchievement;
using OutOfSchool.WebApi.Models.Ministry;

namespace OutOfSchool.WebApi.Controllers.V1;
[Route("api/[controller]")]
[ApiController]
public class MinistryController : ControllerBase
{
    private readonly IMinistryService service;

    public MinistryController(IMinistryService service)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Method for creating a new ministry.
    /// </summary>
    /// <param name="ministryCreationRequestDto">Ministry entity to add.</param>
    /// <returns>Ministry that was created.</returns>
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MinistryCreationResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> Create(MinistryCreationRequestDto ministryCreationRequestDto)
    {
        var newMinistry = await service.Create(ministryCreationRequestDto);
        if (!newMinistry.Succeeded)
        {
            return BadRequest(newMinistry.OperationResult.Errors.ElementAt(0).Description);
        }

        return Created(
            nameof(newMinistry),
            newMinistry);
    }

    /// <summary>
    /// Delete the ministry from the database.
    /// </summary>
    /// <param name="id">The ministry id.</param>
    /// <returns>If deletion was successful, the result will be Status Code 204.</returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await service.Delete(id);
        if (!result.Succeeded)
        {
            return BadRequest(result.OperationResult.Errors.ElementAt(0).Description);
        }

        return NoContent();
    }

    /// <summary>
    /// Get ministry by id from the database.
    /// </summary>
    /// <param name="id">Ministry id to get ministry entity.</param>
    /// <returns>The ministry that was founded.</returns>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MinistryGettingDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("GetById")]
    public async Task<IActionResult> GetById(int id)
    {
        var ministry = await service.GetById(id);
        if (!ministry.Succeeded)
        {
            return BadRequest(ministry.OperationResult.Errors.ElementAt(0).Description);
        }

        if (ministry.Value == null)
        {
            return NoContent();
        }

        return Ok(ministry);
    }

    /// <summary>
    /// Get all ministries.
    /// </summary>
    /// <returns>The ministries that was founded.</returns>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResult<MinistryGettingDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var ministries = await service.GetAll();
        if (!ministries.Succeeded)
        {
            return BadRequest(ministries.OperationResult.Errors.ElementAt(0).Description);
        }

        if (ministries.Value.Count() == 0)
        {
            return NoContent();
        }

        return Ok(ministries);
    }
}
