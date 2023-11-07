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
