using LctMonolith.Models.Database;

namespace GamificationService.Services.Interfaces;

public interface IPlayerService
{
    Task<Player?> GetPlayerByUserIdAsync(string userId);
    Task<Player> CreatePlayerAsync(string userId, string username);
    Task<Player> UpdatePlayerRankAsync(Guid playerId, Guid newRankId);
    Task<Player> AddPlayerExperienceAsync(Guid playerId, int experience);
    Task<Player> AddPlayerManaAsync(Guid playerId, int mana);
    Task<IEnumerable<Player>> GetTopPlayersAsync(int topCount, TimeSpan timeFrame);
    Task<Player> GetPlayerWithProgressAsync(Guid playerId);
}
