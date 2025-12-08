using Domain.ServicesInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeautyZone.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProcedureController : ControllerBase
{
    private readonly IProcedureService _procedureService;
    public ProcedureController(IProcedureService procedureService)
    {
        _procedureService = procedureService;
    }
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("API работает");
    }
}