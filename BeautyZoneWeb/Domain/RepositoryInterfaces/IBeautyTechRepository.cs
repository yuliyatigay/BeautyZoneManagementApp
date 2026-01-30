using Domain.Models;

namespace Domain.RepositoryInterfaces;

public interface IBeautyTechRepository
{
    Task<List<BeautyTech>> FetchAllBeautyTechs();
    Task AddBeautyTechAsync(BeautyTech beautyTech);
    Task<BeautyTech> GetBeautyTechById(Guid id);
    Task<BeautyTech> GetBeautyTechByPhoneNumber(string number);
    Task<List<BeautyTech>> FetchBeautyTechsByProcedureName(string procedureName);
    Task UpdateBeautyTechAsync(BeautyTech beautyTech);
    Task DeleteBeautyTechAsync(BeautyTech beautyTech);
}