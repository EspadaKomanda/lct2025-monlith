using LctMonolith.Database.UnitOfWork;
using LctMonolith.Models.Database;
using LctMonolith.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LctMonolith.Services;

public class InventoryService : IInventoryService
{
    private readonly IUnitOfWork _uow;
    public InventoryService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<UserInventoryItem>> GetStoreInventoryAsync(Guid userId, CancellationToken ct = default)
    {
        try
        {
            return await _uow.UserInventoryItems.Query(i => i.UserId == userId, null, i => i.StoreItem).ToListAsync(ct);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetStoreInventoryAsync failed {UserId}", userId);
            throw;
        }
    }
}
