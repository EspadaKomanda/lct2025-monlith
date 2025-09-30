namespace LctMonolith.Models.Database;

public class EventLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public EventType Type { get; set; }
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public string? Data { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
