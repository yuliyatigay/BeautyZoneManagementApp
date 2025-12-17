using BeautyZone.Dtos;
using Domain.Models;
using Domain.ServicesInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautyZone.Controllers;
[Authorize(Roles = "admin")]
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
    [Route("GetAllProcedures")]
    public async Task<IActionResult> GetAllProcedures()
    {
        var procedures = await _procedureService.GetAllProcedures();
        if (procedures.Count == 0) 
            return NotFound("No procedures found in database");
        return Ok(procedures);
    }
    [HttpGet]
    [Route("GetProcedureById/{id}")]
    public async Task<IActionResult> GetProcedureById(Guid id)
    {
        var procedure = await _procedureService.GetProcedureById(id);
        if (procedure == null) 
            return NotFound("Procedure not found");
        return Ok(procedure);
    }

    [HttpPost]
    [Route("CreateProcedure")]
    public async Task<IActionResult> CreateProcedure([FromBody] ProcedureDto procedure)
    {
        if (procedure == null) 
            return BadRequest("Procedure data must be provided");
        var created = await _procedureService.CreateProcedure(
            new Procedure
            {
                Name = procedure.Name
            });
        if (created == null) 
            return BadRequest("Procedure not created.");
        return CreatedAtAction(nameof(GetProcedureById), new { id = created.Id }, created);
    }
    [HttpPut]
    [Route("UpdateProcedure/{id}")]
    public async Task<IActionResult> UpdateProcedure(Guid id,[FromBody] ProcedureDto procedureDto)
    {
        if (procedureDto == null) 
            return BadRequest("Procedure data must be provided");
        var updated = new Procedure
        {
            Name = procedureDto.Name,
            Id = id
        };
        await _procedureService.UpdateProcedure(updated);
        return Ok(updated);
    }
    [HttpDelete]
    [Route("DeleteProcedure/{id}")]
    public async Task<IActionResult> DeleteProcedure(Guid id)
    {
        var procedure = new Procedure { Id = id };
        await _procedureService.DeleteProcedure(procedure);
        return Ok();
    }
}