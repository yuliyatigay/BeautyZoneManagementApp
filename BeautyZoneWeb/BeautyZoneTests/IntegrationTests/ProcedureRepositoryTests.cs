using DataAccess.Data;
using DataAccess.Repositories;
using Domain.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace BeautyZoneTests.IntegrationTests;

public class ProcedureRepositoryTests : IAsyncLifetime
{
    private PostgreSqlContainer _container { get; } = new PostgreSqlBuilder().
        WithImage("postgres:15").WithDatabase("beautyzone").
        WithUsername("postgres").WithPassword("password").Build();
    private IDbContextFactory<AppDbContext> _contextFactory;
    private ProcedureRepository _repository;
    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        var connectionString = _container.GetConnectionString();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        _contextFactory = new TestDbContextFactory(options);
        _repository = new ProcedureRepository(_contextFactory);

        await using var context = _contextFactory.CreateDbContext();
        await context.Database.MigrateAsync();
    }

    [Fact]
    public async Task CreateProcedure_ShouldCreateProcedureInDb()
    {
        var procedure = new Procedure()
        {
            Name = "NailService"
        };

        await _repository.CreateProcedure(procedure);
        var procedures = await _repository.GetAllProcedures();
        
        procedures.Should().Contain(p => p.Name == procedure.Name);
    }

    [Fact]
    public async Task GetAllProcedures_ShouldReturnProcedures()
    {
        var procedures = new List<Procedure>
        {
            new Procedure { Name = "NailService" },
            new Procedure { Name = "Haircut" }
        };
        foreach (var procedure in procedures)
        {
            await _repository.CreateProcedure(procedure);
        }

        var actual = await _repository.GetAllProcedures();
        actual.Should().NotBeEmpty();
        actual.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetProcedureById_ShouldReturnProcedure()
    {
        var id = Guid.NewGuid();
        var procedures = new List<Procedure>
        {
            new Procedure { Name = "NailService", Id = id},
            new Procedure { Name = "Haircut" }
        };
        foreach (var procedure in procedures)
        {
            await _repository.CreateProcedure(procedure);
        }
        var actual = await _repository.GetProcedureById(id);
        actual.Name.Should().Be("NailService");
    }

    [Fact]
    public async Task UpdateProcedure_ShouldUpdateProcedure()
    {
        var id = Guid.NewGuid();
        var procedure = new Procedure { Name = "NailService", Id = id };
        await _repository.CreateProcedure(procedure);
        procedure.Name = "Haircut";
        await _repository.UpdateProcedure(procedure);
        var actual = await _repository.GetProcedureById(id);
        actual.Name.Should().Be("Haircut");
    }
    [Fact]
    public async Task DeleteProcedure_ShouldDeleteProcedure()
    {
        var id = Guid.NewGuid();
        var procedure = new Procedure { Name = "NailService", Id = id };
        await _repository.CreateProcedure(procedure);
        await _repository.DeleteProcedure(procedure);
        var actual = await _repository.GetProcedureById(id);
        actual.Should().BeNull();
    }


    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}
