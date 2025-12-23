using DataAccess.Data;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    public AccountRepository(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    public async Task CreateAccount(Account account)
    {
        var context = _dbContextFactory.CreateDbContext();
        await context.Accounts.AddAsync(account);
        await context.SaveChangesAsync();
    }

    public async Task<Account> GetByEmail(string email)
    {
        var context = _dbContextFactory.CreateDbContext();
        return await context.Accounts.
            FirstOrDefaultAsync(a => a.Email == email);
    }
}