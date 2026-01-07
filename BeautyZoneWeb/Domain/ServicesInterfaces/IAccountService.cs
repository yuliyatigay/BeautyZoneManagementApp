using Domain.Enums;
using Domain.Models;

namespace Domain.ServicesInterfaces;

public interface IAccountService
{
    Task RegisterAsync(Account account);
    Task<string> LoginAsync(string email, string password);
    Task UpdateAccount(Account account);
    Task<List<Account>> GetAllAccounts();
    Task<Account> GetById(Guid id);
    Task DeleteAccount(Guid id);
}