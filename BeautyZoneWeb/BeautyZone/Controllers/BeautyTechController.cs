using BeautyZone.Dtos;
using Domain.Models;
using Domain.ServicesInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautyZone.Controllers;
[Authorize(Roles = "admin")]
[Route("api/[controller]")]
[ApiController]
public class BeautyTechController : ControllerBase
{
    private readonly IBeautyTechService _beautyTechService;
    private readonly IProcedureService _procedureService;
    public BeautyTechController(IBeautyTechService beautyTechService, IProcedureService procedureService)
    {
        _beautyTechService = beautyTechService;
        _procedureService = procedureService;
    }

    [HttpGet]
    [Route("FetchAllBeautyTechs")]
    public async Task<IActionResult> GetAllEmployees()
    {
        var employees = await _beautyTechService.FetchAllBeautyTechs();
        if (employees.Count == 0)
            return NotFound("No beauty technicians in database");
        return Ok(employees);
    }

    [HttpGet]
    [Route("GetBeautyTechById/{Id}")]
    public async Task<IActionResult> GetEmployeeById(Guid Id)
    {
        var beautyTech = await _beautyTechService.GetBeautyTechById(Id);
        if (beautyTech == null)
            return NotFound("beauty technician not found");
        return Ok(beautyTech);
    }

    [HttpPost]
    [Route("AddBeautyTech")]
    public async Task<IActionResult> AddEmployee([FromBody] BeautyTechDto beautyTechDto)
    {
        try
        {
            if (beautyTechDto == null)
                return BadRequest("Beauty technician data must be provided");
            var created = new BeautyTech
            {
                Name = beautyTechDto.Name,
                PhoneNumber = beautyTechDto.PhoneNumber,
                Procedures = beautyTechDto.Procedures
                    .Select(id => new Procedure { Id = id })
                    .ToList()
            };
            await _beautyTechService.AddBeautyTechAsync(created);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            return Conflict(ex.Message);
        }

    }

    [HttpPut]
    [Route("UpdateBeautyTechAsync/{id}")]
    public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] BeautyTechDto beautyTechDto)
    {
        if (beautyTechDto == null)
            return BadRequest("Beauty technician data must be provided");
        var updated = new BeautyTech
        {
            Id = id,
            Name = beautyTechDto.Name,
            PhoneNumber = beautyTechDto.PhoneNumber,
            Procedures = beautyTechDto.Procedures
                .Select(id => new Procedure { Id = id })   
                .ToList()
        };
        await _beautyTechService.UpdateBeautyTechAsync(updated);
        return Ok(updated);
    }
    [HttpDelete]
    [Route("DeleteBeautyTechAsync/{id}")]
    public async Task<IActionResult> DeleteEmployee(Guid id)
    {
        var beautyTech = new BeautyTech { Id = id };
        await _beautyTechService.DeleteBeautyTechAsync(beautyTech);
        return Ok();
    }
}