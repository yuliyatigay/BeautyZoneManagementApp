using Domain.Models;

namespace Domain.RepositoryInterfaces;

public interface IAccountRepository
{
    Task Register(Account account);
    Task<Account> GetByEmail(string email);
}