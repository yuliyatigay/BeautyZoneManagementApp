using DataAccess.Data;
using DataAccess.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace BeautyZoneTests.IntegrationTests;

public class CustomerRepositoryTests : IAsyncLifetime
{
    private PostgreSqlContainer _container { get; } = new PostgreSqlBuilder().
        WithImage("postgres:15").WithDatabase("beautyzone").
        WithUsername("postgres").WithPassword("password").Build();
    private IDbContextFactory<AppDbContext> _contextFactory;
    private CustomerRepository _repository;
    private Guid id;
    private List<Customer> customers;
    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        var connectionString = _container.GetConnectionString();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        _contextFactory = new TestDbContextFactory(options);
        _repository = new CustomerRepository(_contextFactory);

        await using var context = _contextFactory.CreateDbContext();
        await context.Database.MigrateAsync();
    }

    [Fact]
    public async Task GetAllCustomers_ShouldReturnList()
    {
        id = Guid.NewGuid();
        customers = await AddTestData();
        
        var actual = await _repository.GetAllCustomers();
        
        Assert.Equal(customers.Count, actual.Count);
    }

    [Fact]
    public async Task GetCustomerById_ShouldReturnCustomer()
    {
        id = Guid.NewGuid();
        await AddTestData();
        
        var actual = await _repository.GetCustomerById(id);
        
        Assert.Equal("John", actual.Name);
    }

    [Fact]
    public async Task CreateCustomer_ShouldCreateCustomer()
    {
        id = Guid.NewGuid();
        var customer = new Customer { Id = id,Name = "Anna", PhoneNumber = "11111" };
        await _repository.CreateCustomer(customer);
        
        var actual = await _repository.GetCustomerById(customer.Id);
        
        Assert.Equal(customer.Name, actual.Name);
    }

    [Fact]
    public async Task UpdateCustomer_ShouldUpdateCustomer()
    {
        id = Guid.NewGuid();
        await AddTestData();
        
        var customer = await _repository.GetCustomerById(id);
        customer.Name = "Liza";
        await _repository.UpdateCustomer(customer);
        
        var actual = await _repository.GetCustomerById(customer.Id);
        
        Assert.Equal(customer.Name, actual.Name);
    }

    [Fact]
    public async Task DeleteCustomer_ShouldDeleteCustomer()
    {
        id = Guid.NewGuid();
        await AddTestData();
        var customer = await _repository.GetCustomerById(id);
        await _repository.DeleteCustomer(customer);
        
        var actual = await _repository.GetCustomerById(customer.Id);
        
        Assert.Null(actual);
    }

    private async Task<List<Customer>> AddTestData()
    {
        var customerList = new List<Customer>
        {
            new Customer { Name = "Anna", PhoneNumber = "11111" },
            new Customer { Id = id, Name = "John", PhoneNumber = "22222" }
        };
        foreach (var customer in customerList)
        {
            await _repository.CreateCustomer(customer);
        }
        return customerList;
    }

    public async Task DisposeAsync()
    {
        _container.DisposeAsync();
    }
}