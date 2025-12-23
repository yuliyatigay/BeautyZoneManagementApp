using Domain.Models;

namespace Domain.RepositoryInterfaces;

public interface IAccountRepository
{
    Task CreateAccount(Account account);
    Task<Account> GetByEmail(string email);
}