using BeautyZone.Dtos;
using Domain.Models;
using Domain.ServicesInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeautyZone.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    [Route("GetAllEmployees")]
    public async Task<IActionResult> GetAllEmployees()
    {
        var employees = await _employeeService.GetAllEmployees();
        if (employees.Count == 0)
            return NotFound("No employees in database");
        return Ok(employees);
    }

    [HttpGet]
    [Route("GetEmployeeByName/{Id}")]
    public async Task<IActionResult> GetEmployeeByName(Guid Id)
    {
        var employee = await _employeeService.GetEmployeeById(Id);
        if (employee == null)
            return NotFound("Employee not found");
        return Ok(employee);
    }

    [HttpPost]
    [Route("AddEmployee")]
    public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto employeeDto)
    {
        if (employeeDto == null)
            return BadRequest("Employee data must be provided");
        var created = new Employee
        {
            Name = employeeDto.Name
        };
        await _employeeService.CreateEmployee(created);
        return CreatedAtAction(nameof(AddEmployee), new Employee { Name = employeeDto.Name }, created);
    }

    [HttpPut]
    [Route("UpdateEmployee/{id}")]
    public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] EmployeeDto employeeDto)
    {
        if (employeeDto == null)
            return BadRequest("Employee data must be provided");
        var updated = new Employee
        {
            Name = employeeDto.Name,
            Id = id
        };
        await _employeeService.UpdateEmployee(updated);
        return Ok(updated);
    }
    [HttpDelete]
    [Route("DeleteEmployee/{id}")]
    public async Task<IActionResult> DeleteEmployee(Guid id)
    {
        var employee = new Employee { Id = id };
        await _employeeService.DeleteEmployee(employee);
        return Ok();
    }
}