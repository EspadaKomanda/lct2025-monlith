namespace LctMonolith.Models.Database;

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public TransactionType Type { get; set; }
    public Guid? StoreItemId { get; set; }
    public StoreItem? StoreItem { get; set; }
    public int ManaAmount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
