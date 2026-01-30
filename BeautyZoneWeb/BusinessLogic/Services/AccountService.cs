using Domain.Enums;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Domain.ServicesInterfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogic.Services;

public class AccountService : IAccountService
{
    private readonly IJwtService _jwtService;
    private readonly IAccountRepository _accountRepository;
    private readonly IValidator<Account> _accountValidator;
    public AccountService(IJwtService jwtService, IValidator<Account> accountValidator,
        IAccountRepository accountRepository)
    {
        _jwtService = jwtService;
        _accountRepository = accountRepository;
        _accountValidator = accountValidator;
    }
    public async Task RegisterAsync(Account account)
    {
        var accountFromDb = await _accountRepository.GetByEmail(account.Email);
        if (accountFromDb is not null)
            throw new ArgumentException("Account already exists");
        var result = await _accountValidator.ValidateAsync(account);
        if (!result.IsValid)
            throw new ArgumentException(string.Join(",\n", result.Errors.Select
                (e => e.ErrorMessage)));
        var hashed = new PasswordHasher<Account>().HashPassword(account, account.PasswordHash);
        account.PasswordHash = hashed;
        account.Role = UserRole.user;
        await _accountRepository.CreateAccount(account);
    }

    public async Task<UserResponse> LoginAsync(string email, string password)
    {
        var account = await _accountRepository.GetByEmail(email);
        if (account is null)
        {
            throw new ArgumentException("Account is not registered");
        }
        var result = new PasswordHasher<Account>()
            .VerifyHashedPassword(account, account.PasswordHash, password);
        
        if (result == PasswordVerificationResult.Failed)
        {
            throw new ArgumentException("Invalid username or password");
        }
        var token = _jwtService.GenerateJwtToken(account);
        var response = new UserResponse
        {
            AccessToken = token,
            Role = account.Role,
            Email = account.Email,
        };
        return response;
    }

    public async Task UpdateAccount(Account updated)
    {
        await _accountRepository.UpdateAccount(updated);
    }

    public async Task<List<Account>> GetAllAccounts()
    {
        return await _accountRepository.GetAllAccounts();
    }

    public async Task<Account> GetById(Guid id)
    {
        var account = await _accountRepository.GetById(id);
        if (account is null)
            throw new ArgumentException("Account not found");
        return account;
    }

    public async Task DeleteAccount(Guid id)
    {
        var account = await _accountRepository.GetById(id);
        await _accountRepository.DeleteAccount(account);
    }
}