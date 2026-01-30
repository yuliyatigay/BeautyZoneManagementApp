using DataAccess.Data;
using DataAccess.Repositories;
using Domain.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace BeautyZoneTests.IntegrationTests;

public class BeautyTechRepositoryTests : IAsyncLifetime
{
    private PostgreSqlContainer _container { get; } = new PostgreSqlBuilder().
        WithImage("postgres:15").WithDatabase("beautyzone").
        WithUsername("postgres").WithPassword("password").Build();
    private IDbContextFactory<AppDbContext> _contextFactory;
    private BeautyTechRepository _repository;
    private ProcedureRepository _procedureRepository;
    private Guid id;
    private List<Procedure> procedures;
    private List<BeautyTech> beautyTechs;
    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        var connectionString = _container.GetConnectionString();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        _contextFactory = new TestDbContextFactory(options);
        _repository = new BeautyTechRepository(_contextFactory);
        _procedureRepository = new ProcedureRepository(_contextFactory);

        await using var context = _contextFactory.CreateDbContext();
        await context.Database.MigrateAsync();
    }

    [Fact]
    public async Task FetchAllBeautyTechs_ShouldReturnList()
    {
        id = Guid.NewGuid();
        await AddTestData();
        
        var actual = await _repository.FetchAllBeautyTechs();
        
        Assert.NotEmpty(actual);
    }

    [Fact]
    public async Task AddBeautyTech_ShouldAddBeautyTech()
    {
        var beautyTech = new BeautyTech { Name = "John",PhoneNumber = "11111", Procedures = new List<Procedure>()};
        await _repository.AddBeautyTechAsync(beautyTech);
        
        var actual = await _repository.GetBeautyTechById(beautyTech.Id);
        
        Assert.Equal(beautyTech.Name, actual.Name);
    }

    [Fact]
    public async Task GetBeautyTechById_ShouldReturnBeautyTech()
    {
        id = Guid.NewGuid();
        await AddTestData();
        
        var actual = await _repository.GetBeautyTechById(id);
        
        Assert.Equal("Jane", actual.Name);
    }

    [Fact]
    public async Task UpdateBeautyTechAsync_ShouldUpdateBeautyTech()
    {
        id = Guid.NewGuid();
        await AddTestData();
        
        var beautyTech = await _repository.GetBeautyTechById(id);
        beautyTech.Procedures = new List<Procedure>{procedures[2]};
        await _repository.UpdateBeautyTechAsync(beautyTech);
        
        var actual = await _repository.GetBeautyTechById(id);
        
        actual.Procedures.Should().Contain(p => p.Name == procedures[2].Name);
    }
    [Fact]
    public async Task DeleteBeautyTechAsync_ShouldDeleteBeautyTech()
    {
        id = Guid.NewGuid();
        await AddTestData();
        
        var beautyTech = await _repository.GetBeautyTechById(id);
        await _repository.DeleteBeautyTechAsync(beautyTech);
        
        var actual = await _repository.GetBeautyTechById(id);
        
        actual.Should().BeNull();
    }

    private async Task AddTestData()
    {
       procedures = await FetchProcedures();
        beautyTechs = new List<BeautyTech>
        {
            new BeautyTech 
            { Name = "John",PhoneNumber = "11111", 
                Procedures = new List<Procedure> { procedures[0], procedures[1]} },
            new BeautyTech { Id = id,Name = "Jane",PhoneNumber = "22222", 
                Procedures = new List<Procedure> { procedures[2], procedures[1]}}
        };
        foreach (var beautyTech in beautyTechs)
        {
            await _repository.AddBeautyTechAsync(beautyTech);
        }
    }

    private async Task<List<Procedure>> FetchProcedures()
    {
        var procedureList = new List<Procedure>
        {
            new Procedure { Name = "NailService" },
            new Procedure { Name = "Haircut" },
            new Procedure { Name = "Makeup" }
        };
        foreach (var procedure in procedureList)
        {
            await _procedureRepository.CreateProcedure(procedure);
        }

        return await _procedureRepository.GetAllProcedures();
    }

    public async Task DisposeAsync()
    {
        _container.DisposeAsync();
    }
}