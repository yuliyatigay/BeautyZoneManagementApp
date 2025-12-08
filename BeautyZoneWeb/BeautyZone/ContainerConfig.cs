using BusinessLogic.Services;
using DataAccess.Data;
using DataAccess.Repositories;
using Domain.RepositoryInterfaces;
using Domain.ServicesInterfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautyZone;

public static class ContainerConfig
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
       var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContextFactory<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
        
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IProcedureRepository, ProcedureRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IProcedureService, ProcedureService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
    }
    
}