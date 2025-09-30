using LctMonolith.Models.Database;

namespace LctMonolith.Services.Contracts;

public interface IInventoryService
{
    Task<IEnumerable<UserInventoryItem>> GetStoreInventoryAsync(Guid userId, CancellationToken ct = default);
    Task<IEnumerable<UserArtifact>> GetArtifactsAsync(Guid userId, CancellationToken ct = default);
}
