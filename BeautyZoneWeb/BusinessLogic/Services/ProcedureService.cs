using Domain.Models;
using Domain.RepositoryInterfaces;
using Domain.ServicesInterfaces;

namespace BusinessLogic.Services;

public class ProcedureService : IProcedureService
{
    private readonly IProcedureRepository _procedureRepository;
    public ProcedureService(IProcedureRepository procedureRepository)
    {
        _procedureRepository = procedureRepository;
    }
    public async Task<List<Procedure>> GetAllProcedures()
    {
        return await _procedureRepository.GetAllProcedures();
    }

    public async Task<Procedure> CreateProcedure(Procedure procedure)
    {
        var existing = await _procedureRepository.GetProcedureByName(procedure.Name);
        if (existing != null)
            return existing;

        await _procedureRepository.CreateProcedure(procedure);
        return procedure;
    }

    public async Task<Procedure> GetProcedureByName(string procedureName)
    {
        return await _procedureRepository.GetProcedureByName(procedureName);
    }

    public async Task UpdateProcedure(Procedure procedure)
    {
        await _procedureRepository.UpdateProcedure(procedure);
    }

    public async Task DeleteProcedure(Procedure procedure)
    {
        await _procedureRepository.DeleteProcedure(procedure);
    }
}