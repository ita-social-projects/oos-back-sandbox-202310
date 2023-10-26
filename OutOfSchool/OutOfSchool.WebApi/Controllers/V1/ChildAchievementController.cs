using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutOfSchool.WebApi.Models;

namespace OutOfSchool.WebApi.Controllers.V1;
[Route("api/[controller]")]
[ApiController]
public class ChildAchievementController : ControllerBase
{
    private readonly IChildAchievementService service;

    public ChildAchievementController(IChildAchievementService service)
    {
        this.service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(ChildAchievementCreationDto childAchievementCreationDto) {
        var newAchive = await service.CreateAchievement(childAchievementCreationDto);
        return Ok(newAchive);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        await service.DeleteAchievement(id);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(ChildAchievementDto childAchievementDto)
    {
        var updatedAchive = await service.UpdateAchievement(childAchievementDto);
        return Ok(updatedAchive);
    }

    [HttpGet]
    public async Task<IActionResult> GetForChildId(Guid id)
    {
        var childAchievements = service.GetAchievementForChildId(id);
        return Ok(childAchievements);
    }

    [HttpGet]
    public async Task<IActionResult> GetForWorkshopId(Guid id)
    {
        var childAchievements = service.GetAchievementForWorkshopId(id);
        return Ok(childAchievements);
    }

    [HttpGet]
    public async Task<IActionResult> GetForChildIdWorkshopId(Guid childId,Guid workshopId)
    {
        var childAchievements = service.GetAchievementForWorkshopIdChildId(childId, workshopId);
        return Ok(childAchievements);
    }
}
