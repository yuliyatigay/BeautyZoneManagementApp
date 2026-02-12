using Domain.Models;
using Domain.RepositoryInterfaces;
using Domain.ServicesInterfaces;

namespace BusinessLogic.Services;

public class BeautyTechService : IBeautyTechService
{
    private readonly IBeautyTechRepository _beautyTechRepository;
    public BeautyTechService(IBeautyTechRepository beautyTechRepository)
    {
        _beautyTechRepository = beautyTechRepository;
    }
    public async Task<List<BeautyTech>> FetchAllBeautyTechs()
    {
        return await _beautyTechRepository.FetchAllBeautyTechs();
    }

    public async Task<BeautyTech> AddBeautyTechAsync(BeautyTech beautyTech)
    {
        var existing = await _beautyTechRepository.GetBeautyTechByPhoneNumber(beautyTech.PhoneNumber);
        if (existing != null)
            throw new InvalidOperationException("BeautyTech already exist");
        await _beautyTechRepository.AddBeautyTechAsync(beautyTech);
        return beautyTech;
    }

    public async Task<BeautyTech> GetBeautyTechById(Guid Id)
    {
        return await _beautyTechRepository.GetBeautyTechById(Id);
    }

    public async Task<List<BeautyTech>> FetchBeautyTechsByProcedureName(string procedureName)
    {
        return await _beautyTechRepository.FetchBeautyTechsByProcedureName(procedureName);
    }

    public async Task UpdateBeautyTechAsync(BeautyTech beautyTech)
    {
        await _beautyTechRepository.UpdateBeautyTechAsync(beautyTech);
    }

    public async Task DeleteBeautyTechAsync(BeautyTech beautyTech)
    {
        await _beautyTechRepository.DeleteBeautyTechAsync(beautyTech);
    }
}