using LctMonolith.Models.Database;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json;
using LctMonolith.Database.UnitOfWork;
using LctMonolith.Services.Contracts;

namespace LctMonolith.Services;

public class StoreService : IStoreService
{
    private readonly IUnitOfWork _uow;

    public StoreService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<StoreItem>> GetActiveItemsAsync(CancellationToken ct = default)
    {
        try
        {
            return await _uow.StoreItems.Query(i => i.IsActive).ToListAsync(ct);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetActiveItemsAsync failed");
            throw;
        }
    }

    public async Task<UserInventoryItem> PurchaseAsync(Guid userId, Guid itemId, int quantity, CancellationToken ct = default)
    {
        try
        {
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
            var player = await _uow.Players.Query(p => p.UserId == userId).FirstOrDefaultAsync(ct) ?? throw new KeyNotFoundException("Player not found for user");
            var item = await _uow.StoreItems.Query(i => i.Id == itemId && i.IsActive).FirstOrDefaultAsync(ct) ?? throw new KeyNotFoundException("Item not found or inactive");
            var totalPrice = item.Price * quantity;
            if (player.Mana < totalPrice) throw new InvalidOperationException("Insufficient mana");
            if (item.Stock.HasValue && item.Stock.Value < quantity) throw new InvalidOperationException("Insufficient stock");

            player.Mana -= totalPrice;
            if (item.Stock.HasValue) item.Stock -= quantity;

            var inv = await _uow.UserInventoryItems.FindAsync(userId, itemId);
            if (inv == null)
            {
                inv = new UserInventoryItem { UserId = userId, StoreItemId = itemId, Quantity = quantity, AcquiredAt = DateTime.UtcNow };
                await _uow.UserInventoryItems.AddAsync(inv, ct);
            }
            else
            {
                inv.Quantity += quantity;
            }

            await _uow.Transactions.AddAsync(new Transaction { UserId = userId, StoreItemId = itemId, Type = TransactionType.Purchase, ManaAmount = -totalPrice }, ct);
            await _uow.EventLogs.AddAsync(new EventLog { Type = EventType.ItemPurchased, UserId = userId, Data = JsonSerializer.Serialize(new { itemId, quantity, totalPrice }) }, ct);
            await _uow.SaveChangesAsync(ct);
            Log.Information("User {User} purchased {Qty} of {Item}", userId, quantity, itemId);
            return inv;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "PurchaseAsync failed {UserId} {ItemId}", userId, itemId);
            throw;
        }
    }
}
