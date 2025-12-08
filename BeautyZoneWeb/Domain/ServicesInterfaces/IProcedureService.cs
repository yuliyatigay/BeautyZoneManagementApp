using Domain.Models;

namespace Domain.ServicesInterfaces;

public interface IProcedureService
{
    Task<List<Procedure>> GetAllProcedures();
    Task<Procedure> CreateProcedure(Procedure procedure);
    Task<Procedure> GetProcedureByName(string procedureName);
    Task UpdateProcedure(Procedure procedure);
    Task DeleteProcedure(Procedure procedure);
}