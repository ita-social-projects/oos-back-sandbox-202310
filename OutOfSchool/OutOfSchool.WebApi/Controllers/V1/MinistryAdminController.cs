using Elastic.CommonSchema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutOfSchool.WebApi.Common;
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
