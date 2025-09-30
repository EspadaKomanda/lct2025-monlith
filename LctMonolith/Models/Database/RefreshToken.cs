namespace LctMonolith.Models.Database;

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
