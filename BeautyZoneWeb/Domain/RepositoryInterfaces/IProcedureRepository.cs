using Domain.Models;

namespace Domain.RepositoryInterfaces;

public interface IProcedureRepository
{
    Task<List<Procedure>> GetAllProcedures();
    Task CreateProcedure(Procedure procedure);
    Task<Procedure> GetProcedureById(Guid id);
    Task<Procedure> GetProcedureByName(string name);
    Task UpdateProcedure(Procedure procedure);
    Task DeleteProcedure(Procedure procedure);
}