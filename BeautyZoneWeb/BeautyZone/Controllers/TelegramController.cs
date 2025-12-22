using BeautyZone.Dtos;
using Domain.ServicesInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeautyZone.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TelegramController : ControllerBase
{
    private readonly IProcedureService _procedureService;

    public TelegramController(IProcedureService procedureService)
    {
        _procedureService = procedureService;
    }
    [HttpGet]
    [Route("GetProcedures")]
    public async Task<IActionResult> GetAllProcedures()
    {
        var procedures = await _procedureService.GetAllProcedures();
        if (procedures.Count == 0) 
            return NotFound("No procedures found in database");
        var procedureForTelegram = new List<ProcedureDto>();
        foreach (var procedure in procedures)
        {
            procedureForTelegram.Add(new ProcedureDto{ Name = procedure.Name});
        }
        return Ok(procedureForTelegram);
    }
}