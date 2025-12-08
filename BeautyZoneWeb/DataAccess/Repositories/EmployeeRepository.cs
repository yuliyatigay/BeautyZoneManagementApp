using DataAccess.Data;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public EmployeeRepository(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    public async Task<List<Employee>> GetAllEmployees()
    {
        using var context = _dbContextFactory.CreateDbContext();
        return await context.Employees
            .Include(p => p.Procedures)
            .ToListAsync();
    }

    public async Task CreateEmployee(Employee employee)
    {
        using var context = _dbContextFactory.CreateDbContext();
        
        foreach (var procedure in employee.Procedures)
        {
            context.Attach(procedure);
        }

        context.Employees.Add(employee);
        await context.SaveChangesAsync();
    }

    public async Task<Employee> GetEmployeeById(Guid id)
    {
        using var context = _dbContextFactory.CreateDbContext();
        return await context.Employees.
            Include(p => p.Procedures).
            FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<Employee>> GetEmployeesByProcedure(string procedureName)
    {
        using var context = _dbContextFactory.CreateDbContext();
        return await context.Employees.
            Where(e => e.Procedures.
                Any(p => p.Name == procedureName)).
            ToListAsync();
    }


    public async Task UpdateEmployee(Employee master)
    {
        using var context = _dbContextFactory.CreateDbContext();
        context.Employees.Update(master);
        await context.SaveChangesAsync();
    }

    public async Task DeleteEmployee(Employee master)
    {
        using var context = _dbContextFactory.CreateDbContext();
        context.Employees.Remove(master);
        await context.SaveChangesAsync();
    }
}