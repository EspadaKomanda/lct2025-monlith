using LctMonolith.Models;

namespace LctMonolith.Services.Contracts;

public interface IStoreService
{
    Task<IEnumerable<StoreItem>> GetActiveItemsAsync(CancellationToken ct = default);
    Task<UserInventoryItem> PurchaseAsync(Guid userId, Guid itemId, int quantity, CancellationToken ct = default);
}
