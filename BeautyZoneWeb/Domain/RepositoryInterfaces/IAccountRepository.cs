using Domain.Models;

namespace Domain.RepositoryInterfaces;

public interface IAccountRepository
{
    Task CreateAccount(Account account);
    Task<Account> GetByEmail(string email);
    Task UpdateAccount(Account account);
    Task<Account> GetById(Guid id);
    Task<List<Account>> GetAllAccounts();
    Task DeleteAccount(Account account);
}