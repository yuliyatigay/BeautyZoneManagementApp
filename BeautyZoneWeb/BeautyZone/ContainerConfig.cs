using System.Security.Claims;
using System.Text;
using BusinessLogic.Services;
using BusinessLogic.Utilities;
using DataAccess.Data;
using DataAccess.Repositories;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Domain.ServicesInterfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BeautyZone;

public static class ContainerConfig
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
       var connectionString = configuration.GetConnectionString("DefaultConnection");

       services.AddDbContextFactory<AppDbContext>(options =>
       {
           options.UseNpgsql(connectionString)
               .UseLazyLoadingProxies();
       });
       
        
        services.Configure<AuthSettings>(configuration.GetSection("AuthSettings"));
        
        var authConfig = configuration.GetSection(nameof(AuthSettings)).Get<AuthSettings>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.SecretKey)),
                    RoleClaimType = ClaimTypes.Role
                };
            });
        services.AddScoped<IValidator<Account>, PasswordValidator>();
        
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IProcedureRepository, ProcedureRepository>();
        services.AddScoped<IBeautyTechRepository, BeautyTechRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IProcedureService, ProcedureService>();
        services.AddScoped<IBeautyTechService, BeautyTechService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAccountService, AccountService>();
        
    }
    
}