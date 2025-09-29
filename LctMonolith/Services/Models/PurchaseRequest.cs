namespace LctMonolith.Services.Models;

public class PurchaseRequest
{
    public Guid ItemId { get; set; }
    public int Quantity { get; set; } = 1;
}
