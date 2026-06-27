using Domain.Models;

namespace Domain.ServicesInterfaces;

public interface IJwtService
{
    TokenResult GenerateJwtToken(Account account);
}