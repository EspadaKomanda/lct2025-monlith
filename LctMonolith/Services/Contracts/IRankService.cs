using LctMonolith.Models.Database;

namespace GamificationService.Services.Interfaces;

public interface IRankService
{
    Task<Rank?> GetRankByIdAsync(Guid rankId);
    Task<Rank?> GetRankByTitleAsync(string title);
    Task<IEnumerable<Rank>> GetAllRanksAsync();
    Task<Rank> CreateRankAsync(Rank rank);
    Task<Rank> UpdateRankAsync(Rank rank);
    Task<bool> DeleteRankAsync(Guid rankId);
    Task<bool> CanPlayerAdvanceToRankAsync(Guid playerId, Guid rankId);
    Task<Rank?> GetNextRankAsync(Guid currentRankId);
}
