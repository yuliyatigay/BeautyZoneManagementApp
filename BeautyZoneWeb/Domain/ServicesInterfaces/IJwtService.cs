using Domain.Models;

namespace Domain.ServicesInterfaces;

public interface IJwtService
{
    string GenerateJwtToken(Account account);
}