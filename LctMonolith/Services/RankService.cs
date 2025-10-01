using LctMonolith.Database.UnitOfWork;
using LctMonolith.Models.Database;
using LctMonolith.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LctMonolith.Services;

public class RankService : IRankService
{
    private readonly IUnitOfWork _uow;

    public RankService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Rank?> GetRankByIdAsync(Guid rankId)
    {
        try
        {
            return await _uow.Ranks.GetByIdAsync(rankId);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetRankByIdAsync failed {RankId}", rankId);
            throw;
        }
    }

    public async Task<Rank?> GetRankByTitleAsync(string title)
    {
        try
        {
            return await _uow.Ranks.Query(r => r.Title == title).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetRankByTitleAsync failed {Title}", title);
            throw;
        }
    }

    public async Task<IEnumerable<Rank>> GetAllRanksAsync()
    {
        try
        {
            return await _uow.Ranks.Query().OrderBy(r => r.ExpNeeded).ToListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetAllRanksAsync failed");
            throw;
        }
    }

    public async Task<Rank> CreateRankAsync(Rank rank)
    {
        try
        {
            rank.Id = Guid.NewGuid();
            await _uow.Ranks.AddAsync(rank);
            await _uow.SaveChangesAsync();
            return rank;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "CreateRankAsync failed {Title}", rank.Title);
            throw;
        }
    }

    public async Task<Rank> UpdateRankAsync(Rank rank)
    {
        try
        {
            _uow.Ranks.Update(rank);
            await _uow.SaveChangesAsync();
            return rank;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "UpdateRankAsync failed {RankId}", rank.Id);
            throw;
        }
    }

    public async Task<bool> DeleteRankAsync(Guid rankId)
    {
        try
        {
            var r = await _uow.Ranks.GetByIdAsync(rankId);
            if (r == null)
            {
                return false;
            }

            _uow.Ranks.Remove(r);
            await _uow.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "DeleteRankAsync failed {RankId}", rankId);
            throw;
        }
    }

    public async Task<bool> CanPlayerAdvanceToRankAsync(Guid playerId, Guid rankId)
    {
        try
        {
            var player = await _uow.Players.GetByIdAsync(playerId) ?? throw new KeyNotFoundException("Player not found");
            var rank = await _uow.Ranks.GetByIdAsync(rankId) ?? throw new KeyNotFoundException("Rank not found");
            if (player.Experience < rank.ExpNeeded)
            {
                return false;
            }

            var missionReqs = await _uow.RankMissionRules.Query(rmr => rmr.RankId == rankId).Select(r => r.MissionId).ToListAsync();
            if (missionReqs.Count > 0)
            {
                var completed = await _uow.PlayerMissions.Query(pm => pm.PlayerId == playerId && pm.Completed != null).Select(pm => pm.MissionId).ToListAsync();
                if (missionReqs.Except(completed).Any())
                {
                    return false;
                }
            }

            var skillReqs = await _uow.RankSkillRules.Query(rsr => rsr.RankId == rankId).ToListAsync();
            if (skillReqs.Count > 0)
            {
                var playerSkills = await _uow.PlayerSkills.Query(ps => ps.PlayerId == playerId).ToListAsync();
                foreach (var req in skillReqs)
                {
                    var ps = playerSkills.FirstOrDefault(s => s.SkillId == req.SkillId);
                    if (ps == null || ps.Score < req.Min)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "CanPlayerAdvanceToRankAsync failed {PlayerId}->{RankId}", playerId, rankId);
            throw;
        }
    }

    public async Task<Rank?> GetNextRankAsync(Guid currentRankId)
    {
        try
        {
            var current = await _uow.Ranks.GetByIdAsync(currentRankId);
            if (current == null)
            {
                return null;
            }

            return await _uow.Ranks.Query(r => r.ExpNeeded > current.ExpNeeded).OrderBy(r => r.ExpNeeded).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetNextRankAsync failed {RankId}", currentRankId);
            throw;
        }
    }
}
