using BeautyZone.Dtos;
using Domain.Models;
using Domain.ServicesInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautyZone.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    [HttpPost("CreateAccount")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        var account = new Account
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            Email = request.Email,
            PasswordHash = request.Password
        };
        try
        {
            await _accountService.RegisterAsync(account);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        try
        {
            var result = await _accountService.LoginAsync(login.Email, login.Password);
            return Ok(result);
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [Authorize(Roles = "admin")]
    [HttpPut("UpdateAccount/{Id}")]
    public async Task<IActionResult> ChangeRole(Guid id, [FromBody] UserDto dto)
    {
        var updated = new Account
        {
            Id = id,
            FirstName = dto.FirstName,
            UserName = dto.UserName,
            LastName = dto.LastName,
            Email = dto.Email,
            Role = dto.Role
        };
        try
        {
            await _accountService.UpdateAccount(updated);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "admin")]
    [HttpGet("GetAccountById/{Id}")]
    public async Task<IActionResult> GetAccountById(Guid id)
    {
        var account = await _accountService.GetById(id);
        if (account == null) return NotFound("Account not found");
        var user = new UserDto
        {
            Id = id,
            FirstName = account.FirstName,
            LastName = account.LastName,
            UserName = account.UserName,
            Role = account.Role,
            Email = account.Email
        };
        return Ok(user);
    }

    [Authorize(Roles = "admin")]
    [HttpGet ("GetAllAccounts")]

    public async Task<IActionResult> GetAllAccounts()
    {
        var accounts = await _accountService.GetAllAccounts();
        if (accounts.Count == 0) return NotFound("No accounts found");
        var result = accounts.Select(x => new UserDto {Id = x.Id, FirstName = x.FirstName + " " + x.LastName, Role = x.Role, Email = x.Email});
        return Ok(result);
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("DeleteAccount/{Id}")]
    public async Task<IActionResult> DeleteAccount(Guid id)
    {
        await _accountService.DeleteAccount(id);
        return Ok();
    }
}