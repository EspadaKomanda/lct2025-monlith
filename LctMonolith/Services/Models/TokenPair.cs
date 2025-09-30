namespace LctMonolith.Services.Models;

public record TokenPair(string AccessToken, DateTime AccessTokenExpiresAt, string RefreshToken, DateTime RefreshTokenExpiresAt);
