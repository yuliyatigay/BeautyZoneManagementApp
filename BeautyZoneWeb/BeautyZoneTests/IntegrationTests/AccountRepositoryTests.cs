using DataAccess.Data;
using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace BeautyZoneTests.IntegrationTests;

public class AccountRepositoryTests : IAsyncLifetime
{
    private PostgreSqlContainer _container { get; } = new PostgreSqlBuilder().
        WithImage("postgres:15").WithDatabase("beautyzone").
        WithUsername("postgres").WithPassword("password").Build();
    private IDbContextFactory<AppDbContext> _contextFactory;
    private AccountRepository _repository;
    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        var connectionString = _container.GetConnectionString();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        _contextFactory = new TestDbContextFactory(options);
        _repository = new AccountRepository(_contextFactory);

        await using var context = _contextFactory.CreateDbContext();
        await context.Database.MigrateAsync();
    }

    [Fact]
    public async Task CreateAccount_ShouldCreateAccountInDb()
    {
        var account = new Account
        {
            FirstName = "John", LastName = "Doe",
            UserName = "johndoe",
            Email = "johdoe@gmail.com", PasswordHash = "password"
        };
        await _repository.CreateAccount(account);
        
        var actual = await _repository.GetByEmail(account.Email);
        
        Assert.NotNull(actual);
    }

    [Fact]
    public async Task GetByEmail_ShouldReturnAccount()
    {
        var account = new Account
        {
            FirstName = "John", LastName = "Doe",
            UserName = "johndoe",
            Email = "johdoe@gmail.com", PasswordHash = "password"
        };
        await _repository.CreateAccount(account);
        
        var actual = await _repository.GetByEmail(account.Email);
        
        Assert.Equal(account.Email, actual.Email);
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}