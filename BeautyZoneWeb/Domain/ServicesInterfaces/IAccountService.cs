using Domain.Models;

namespace Domain.ServicesInterfaces;

public interface IAccountService
{
    Task RegisterAsync(Account account);
    Task<string> LoginAsync(string email, string password);
}