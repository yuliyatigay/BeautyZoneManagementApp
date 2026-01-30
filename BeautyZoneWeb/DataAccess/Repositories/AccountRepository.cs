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
    public async Task UpdateAccount(Account account)
    {
        var context = _dbContextFactory.CreateDbContext();
        var existing = await context.Accounts.FindAsync(account.Id);
        existing.Email = account.Email;
        existing.FirstName = account.FirstName;
        existing.LastName = account.LastName;
        existing.Role = account.Role;
        existing.UserName =  account.UserName;
        await context.SaveChangesAsync();
    }

    public async Task<Account> GetById(Guid id)
    {
        var context = _dbContextFactory.CreateDbContext();
        return await context.Accounts.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Account>> GetAllAccounts()
    {
        var context = _dbContextFactory.CreateDbContext();
        return await context.Accounts.ToListAsync();
    }

    public async Task DeleteAccount(Account account)
    {
        var context = _dbContextFactory.CreateDbContext();
        context.Accounts.Remove(account);
        await context.SaveChangesAsync();
    }
}