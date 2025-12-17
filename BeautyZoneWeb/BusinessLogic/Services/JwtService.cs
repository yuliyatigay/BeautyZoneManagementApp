using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Models;
using Domain.ServicesInterfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLogic.Services;

public class JwtService : IJwtService
{
    private readonly IOptions<AuthSettings> _authSettings;

    public JwtService(IOptions<AuthSettings> authSettings)
    {
        _authSettings = authSettings;
    }

    public string GenerateJwtToken(Account account)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
            new Claim(ClaimTypes.GivenName, account.FirstName),
            new Claim(ClaimTypes.Surname, account.LastName),
            new Claim(ClaimTypes.Role, account.Role.ToString()),
            new Claim(ClaimTypes.Email, account.Email),
            new Claim(ClaimTypes.Name, account.UserName)
        };
        var jwtToken = new JwtSecurityToken(
            expires: DateTime.UtcNow.Add(_authSettings.Value.expires),
            claims: claims,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(_authSettings.Value.SecretKey)),
                SecurityAlgorithms.HmacSha256Signature));
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}
public class AuthSettings
{
    public TimeSpan expires { get; set; }
    public string SecretKey { get; set; }
}