using BeautyZone.Dtos;
using Domain.Models;
using Domain.ServicesInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeautyZone.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IProcedureService _procedureService;
    public CustomerController(ICustomerService customerService, IProcedureService procedureService)
    {
        _customerService = customerService;
        _procedureService = procedureService;
    }
    [HttpGet]
    [Route("GetAllCustomers")]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await _customerService.GetAllCustomers();
        if (customers.Count == 0) 
            return NotFound("No customers found in database");
        return Ok(customers);
    }
    [HttpGet]
    [Route("GetCustomerByPhonenumber/{number}")]
    public async Task<IActionResult> GetCustomerByPhonenumber(string number)
    {
        var customer = await _customerService.GetCustomerByPhonenumber(number);
        if (customer == null) 
            return NotFound("Customer not found");
        return Ok(customer);
    }

    [HttpPost]
    [Route("CreateCustomer")]
    public async Task<IActionResult> AddCustomer([FromBody] CustomerDto customerDto)
    {
        if (customerDto == null) 
            return BadRequest("Customer data must be provided");
        var procedures = new List<Procedure>();
        foreach (var id in customerDto.Procedures)
        {
            procedures.Add(await _procedureService.GetProcedureById(id));
        }

        var created = new Customer
        {
            Name = customerDto.Name,
            PhoneNumber = customerDto.PhoneNumber,
            TelegramId = customerDto.TelegramId,
            Procedures = procedures
        };
        await _customerService.CreateCustomer(created);
        return CreatedAtAction(nameof(AddCustomer), new Customer { Name = customerDto.Name }, created);
    }

    [HttpPut]
    [Route("UpdateCustomer/{id}")]
    public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] CustomerDto customerDto)
    {
        if (customerDto == null)
            return BadRequest("No data was provided");
        var procedures = new List<Procedure>();
        foreach (var procedureId in customerDto.Procedures)
        {
            procedures.Add(await _procedureService.GetProcedureById(procedureId));
        }
        var updated = new Customer
        {
            Name = customerDto.Name,
            PhoneNumber = customerDto.PhoneNumber,
            TelegramId = customerDto.TelegramId,
            Id = id,
            Procedures = procedures
        };
        await _customerService.UpdateCustomer(updated);
        return Ok(updated);
    }
    [HttpDelete]
    [Route("DeleteCustomer/{id}")]
    public async Task<IActionResult> DeleteCustomer(Guid id)
    {
        var customer = new Customer { Id = id };
        await _customerService.DeleteCustomer(customer);
        return Ok();
    }

}