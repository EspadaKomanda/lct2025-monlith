using LctMonolith.Database.UnitOfWork;
using LctMonolith.Models.Database;
using LctMonolith.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LctMonolith.Services;

public class PlayerService : IPlayerService
{
    private readonly IUnitOfWork _uow;
    public PlayerService(IUnitOfWork uow) => _uow = uow;

    public async Task<Player?> GetPlayerByUserIdAsync(string userId)
    {
        try
        {
            if (!Guid.TryParse(userId, out var uid)) return null;
            return await _uow.Players.Query(p => p.UserId == uid, null, p => p.Rank).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetPlayerByUserIdAsync failed {UserId}", userId);
            throw;
        }
    }

    public async Task<Player> CreatePlayerAsync(string userId, string username)
    {
        try
        {
            if (!Guid.TryParse(userId, out var uid)) throw new ArgumentException("Invalid user id", nameof(userId));
            var existing = await GetPlayerByUserIdAsync(userId);
            if (existing != null) return existing;
            // pick lowest exp rank
            var baseRank = await _uow.Ranks.Query().OrderBy(r => r.ExpNeeded).FirstAsync();
            var player = new Player
            {
                Id = Guid.NewGuid(),
                UserId = uid,
                RankId = baseRank.Id,
                Experience = 0,
                Mana = 0
            };
            await _uow.Players.AddAsync(player);
            await _uow.SaveChangesAsync();
            Log.Information("Created player {PlayerId} for user {UserId}", player.Id, userId);
            return player;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "CreatePlayerAsync failed {UserId}", userId);
            throw;
        }
    }

    public async Task<Player> UpdatePlayerRankAsync(Guid playerId, Guid newRankId)
    {
        try
        {
            var player = await _uow.Players.GetByIdAsync(playerId) ?? throw new KeyNotFoundException("Player not found");
            var rank = await _uow.Ranks.GetByIdAsync(newRankId) ?? throw new KeyNotFoundException("Rank not found");
            player.RankId = rank.Id;
            await _uow.SaveChangesAsync();
            return player;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "UpdatePlayerRankAsync failed {PlayerId} -> {RankId}", playerId, newRankId);
            throw;
        }
    }

    public async Task<Player> AddPlayerExperienceAsync(Guid playerId, int experience)
    {
        try
        {
            if (experience == 0) return await _uow.Players.GetByIdAsync(playerId) ?? throw new KeyNotFoundException();
            var player = await _uow.Players.GetByIdAsync(playerId) ?? throw new KeyNotFoundException("Player not found");
            player.Experience += experience;
            await _uow.SaveChangesAsync();
            await AutoRankUpAsync(player);
            return player;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "AddPlayerExperienceAsync failed {PlayerId}", playerId);
            throw;
        }
    }

    private async Task AutoRankUpAsync(Player player)
    {
        // find highest rank whose ExpNeeded <= player's experience
        var target = await _uow.Ranks.Query(r => r.ExpNeeded <= player.Experience)
            .OrderByDescending(r => r.ExpNeeded)
            .FirstOrDefaultAsync();
        if (target != null && target.Id != player.RankId)
        {
            player.RankId = target.Id;
            await _uow.SaveChangesAsync();
            Log.Information("Player {Player} advanced to rank {Rank}", player.Id, target.Title);
        }
    }

    public async Task<Player> AddPlayerManaAsync(Guid playerId, int mana)
    {
        try
        {
            if (mana == 0) return await _uow.Players.GetByIdAsync(playerId) ?? throw new KeyNotFoundException();
            var player = await _uow.Players.GetByIdAsync(playerId) ?? throw new KeyNotFoundException("Player not found");
            player.Mana += mana;
            await _uow.SaveChangesAsync();
            return player;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "AddPlayerManaAsync failed {PlayerId}", playerId);
            throw;
        }
    }

    public async Task<IEnumerable<Player>> GetTopPlayersAsync(int topCount, TimeSpan timeFrame)
    {
        try
        {
            // Simple ordering by experience (timeFrame ignored due to no timestamp on Player)
            return await _uow.Players.Query()
                .OrderByDescending(p => p.Experience)
                .Take(topCount)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetTopPlayersAsync failed");
            throw;
        }
    }

    public async Task<Player> GetPlayerWithProgressAsync(Guid playerId)
    {
        try
        {
            return await _uow.Players.Query(p => p.Id == playerId, null, p => p.Rank)
                .FirstOrDefaultAsync() ?? throw new KeyNotFoundException("Player not found");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetPlayerWithProgressAsync failed {PlayerId}", playerId);
            throw;
        }
    }
}
