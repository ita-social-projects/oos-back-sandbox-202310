using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
}
