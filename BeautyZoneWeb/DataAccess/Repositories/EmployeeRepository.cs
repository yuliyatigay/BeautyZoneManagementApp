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
        var context = _dbContextFactory.CreateDbContext();
        return await context.Employees
            .Include(p => p.Procedures)
            .ToListAsync();
    }

    public async Task CreateEmployee(Employee employee)
    {
        var context = _dbContextFactory.CreateDbContext();
        
        foreach (var procedure in employee.Procedures)
        {
            context.Attach(procedure);
        }

        context.Employees.AddAsync(employee);
        await context.SaveChangesAsync();
    }

    public async Task<Employee> GetEmployeeById(Guid id)
    {
        var context = _dbContextFactory.CreateDbContext();
        return await context.Employees.
            Include(p => p.Procedures).
            FirstOrDefaultAsync(e => e.Id == id);
    }

    public Task<Employee> GetEmployeeByPhonenumber(string number)
    {
        var context = _dbContextFactory.CreateDbContext();
        return context.Employees
            .Include(e => e.Procedures)
            .FirstOrDefaultAsync(e => e.PhoneNumber == number);
    }

    public async Task<List<Employee>> GetEmployeesByProcedure(string procedureName)
    {
        var context = _dbContextFactory.CreateDbContext();
        return await context.Employees.
            Where(e => e.Procedures.
                Any(p => p.Name == procedureName)).
            ToListAsync();
    }


    public async Task UpdateEmployee(Employee master)
    {
        var context = _dbContextFactory.CreateDbContext();

        var existing = await context.Employees
            .Include(e => e.Procedures)
            .FirstOrDefaultAsync(e => e.Id == master.Id);
        existing.Name = master.Name;
        existing.PhoneNumber = master.PhoneNumber;

        existing.Procedures.Clear();

        foreach (var pid in master.Procedures.Select(p => p.Id).Distinct())
        {
            var procedure = await context.Procedures.FindAsync(pid);
            if (procedure != null)
            {
                existing.Procedures.Add(procedure);
            }
        }
        await context.SaveChangesAsync();
    }

    public async Task DeleteEmployee(Employee master)
    {
        var context = _dbContextFactory.CreateDbContext();
        context.Employees.Remove(master);
        await context.SaveChangesAsync();
    }
}