namespace LctMonolith.Models.Database;

public class StoreItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int Price { get; set; }
    public bool IsActive { get; set; } = true;
    public int? Stock { get; set; }
    public ICollection<UserInventoryItem> UserInventory { get; set; } = new List<UserInventoryItem>();
}
