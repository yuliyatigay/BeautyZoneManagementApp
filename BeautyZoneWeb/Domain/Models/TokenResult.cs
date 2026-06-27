namespace Domain.Models;

public record TokenResult(
    string AccessToken,
    DateTime ExpiryTime);