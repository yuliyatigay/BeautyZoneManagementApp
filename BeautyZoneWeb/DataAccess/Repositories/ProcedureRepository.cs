using DataAccess.Data;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class ProcedureRepository : IProcedureRepository
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public ProcedureRepository(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    public async Task<List<Procedure>> GetAllProcedures()
    {
        var context = _dbContextFactory.CreateDbContext();
        return await context.Procedures.ToListAsync();
    }

    public async Task CreateProcedure(Procedure procedure)
    {
        using var context = _dbContextFactory.CreateDbContext();
        await context.Procedures.AddAsync(procedure);
        await context.SaveChangesAsync();
    }

    public async Task<Procedure> GetProcedureById(Guid id)
    {
        using var context = _dbContextFactory.CreateDbContext();
        return await context.Procedures
                .Include(p => p.BeautyTechs)
                .Include(p => p.Customers)
                .FirstOrDefaultAsync(p => p.Id == id);
        
    }

    public async Task<Procedure> GetProcedureByName(string name)
    {
        using var context = _dbContextFactory.CreateDbContext();
        return await context.Procedures
            .Include(p => p.BeautyTechs)
            .Include(p => p.Customers)
            .FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task UpdateProcedure(Procedure procedure)
    {
        using var context = _dbContextFactory.CreateDbContext();
        context.Procedures.Update(procedure);
        await context.SaveChangesAsync();
    }

    public async Task DeleteProcedure(Procedure procedure)
    {
        using var context = _dbContextFactory.CreateDbContext();
        context.Procedures.Remove(procedure);
        await context.SaveChangesAsync();
    }
}