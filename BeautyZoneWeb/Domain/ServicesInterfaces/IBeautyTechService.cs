using Domain.Models;

namespace Domain.ServicesInterfaces;

public interface IBeautyTechService
{
    Task<List<BeautyTech>> FetchAllBeautyTechs();
    Task<BeautyTech> AddBeautyTechAsync(BeautyTech beautyTech);
    Task<BeautyTech> GetBeautyTechById(Guid Id);
    Task<List<BeautyTech>> FetchBeautyTechsByProcedureName(string procedureName);
    Task UpdateBeautyTechAsync(BeautyTech beautyTech);
    Task DeleteBeautyTechAsync(BeautyTech beautyTech);
}