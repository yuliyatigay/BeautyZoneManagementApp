using Domain.Enums;

namespace Domain.Models;

public class UserResponse
{
    public string? Email  { get; set; }
    public string AccessToken { get; set; }
    public UserRole Role { get; set; }
}