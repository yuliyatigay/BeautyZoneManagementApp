using DataAccess.Data;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public CustomerRepository(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    public async Task<List<Customer>> GetAllCustomers()
    {
        using var context = _dbContextFactory.CreateDbContext();
        return await context.Customers.ToListAsync();
    }

    public async Task<Customer> GetCustomerByPhonenumber(string number)
    {
        using var context = _dbContextFactory.CreateDbContext();
        return await context.Customers
            .FirstOrDefaultAsync(c => c.PhoneNumber == number);
    }

    public async Task CreateCustomer(Customer customer)
    {
        using var context = _dbContextFactory.CreateDbContext();
        foreach (var procedure in customer.Procedures)
        {
            context.Attach(procedure);
        }
        await context.Customers.AddAsync(customer);
        await context.SaveChangesAsync();
    }

    public async Task UpdateCustomer(Customer customer)
    {
        using var context = _dbContextFactory.CreateDbContext();
        context.Customers.Update(customer);
        await context.SaveChangesAsync();
    }

    public async Task DeleteCustomer(Customer customer)
    {
        using var context = _dbContextFactory.CreateDbContext();
        context.Customers.Remove(customer);
        await context.SaveChangesAsync();
    }
}