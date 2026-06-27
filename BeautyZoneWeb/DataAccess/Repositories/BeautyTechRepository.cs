using DataAccess.Data;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class BeautyTechRepository : IBeautyTechRepository
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public BeautyTechRepository(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    public async Task<List<BeautyTech>> FetchAllBeautyTechs()
    {
        var context = _dbContextFactory.CreateDbContext();
        return await context.BeautyTechs
            .Include(p => p.Procedures)
            .ToListAsync();
    }

    public async Task AddBeautyTechAsync(BeautyTech specialist)
    {
        var context = _dbContextFactory.CreateDbContext();
        
        foreach (var procedure in specialist.Procedures)
        {
            context.Attach(procedure);
        }

        context.BeautyTechs.AddAsync(specialist);
        await context.SaveChangesAsync();
    }

    public async Task<BeautyTech> GetBeautyTechById(Guid id)
    {
        var context = _dbContextFactory.CreateDbContext();
        return await context.BeautyTechs.
            Include(p => p.Procedures).
            FirstOrDefaultAsync(e => e.Id == id);
    }

    public Task<BeautyTech> GetBeautyTechByPhoneNumber(string number)
    {
        var context = _dbContextFactory.CreateDbContext();
        return context.BeautyTechs
            .Include(e => e.Procedures)
            .FirstOrDefaultAsync(e => e.PhoneNumber == number);
    }

    public async Task<List<BeautyTech>> FetchBeautyTechsByProcedureName(string procedureName)
    {
        var context = _dbContextFactory.CreateDbContext();
        return await context.BeautyTechs.
            Where(e => e.Procedures.
                Any(p => p.Name == procedureName)).
            ToListAsync();
    }


    public async Task UpdateBeautyTechAsync(BeautyTech specialist)
    {
        var context = _dbContextFactory.CreateDbContext();

        var existing = await context.BeautyTechs
            .Include(e => e.Procedures)
            .FirstOrDefaultAsync(e => e.Id == specialist.Id);
        existing.Name = specialist.Name;
        existing.PhoneNumber = specialist.PhoneNumber;

        existing.Procedures.Clear();

        foreach (var pid in specialist.Procedures.Select(p => p.Id).Distinct())
        {
            var procedure = await context.Procedures.FindAsync(pid);
            if (procedure != null)
            {
                existing.Procedures.Add(procedure);
            }
        }
        await context.SaveChangesAsync();
    }

    public async Task DeleteBeautyTechAsync(BeautyTech specialist)
    {
        var context = _dbContextFactory.CreateDbContext();
        context.BeautyTechs.Remove(specialist);
        await context.SaveChangesAsync();
    }
}