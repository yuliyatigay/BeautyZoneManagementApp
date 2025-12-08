using Domain.Models;

namespace Domain.ServicesInterfaces;

public interface IProcedureService
{
    Task<List<Procedure>> GetAllProcedures();
    Task<Procedure> CreateProcedure(Procedure procedure);
    Task<Procedure> GetProcedureById(Guid id);
    Task UpdateProcedure(Procedure procedure);
    Task DeleteProcedure(Procedure procedure);
}