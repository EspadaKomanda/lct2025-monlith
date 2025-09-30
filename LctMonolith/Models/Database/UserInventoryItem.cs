namespace LctMonolith.Models.Database;

public class UserInventoryItem
{
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public Guid StoreItemId { get; set; }
    public StoreItem StoreItem { get; set; } = null!;
    public int Quantity { get; set; } = 1;
    public DateTime AcquiredAt { get; set; } = DateTime.UtcNow;
    public bool IsReturned { get; set; }
}
