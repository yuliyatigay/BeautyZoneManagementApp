using Domain.Models;

namespace Domain.RepositoryInterfaces;

public interface IProcedureRepository
{
    Task<List<Procedure>> GetAllProcedures();
    Task CreateProcedure(Procedure procedure);
    Task<Procedure> GetProcedureByName(string procedureName);
    Task UpdateProcedure(Procedure procedure);
    Task DeleteProcedure(Procedure procedure);
}