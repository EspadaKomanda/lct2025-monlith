using LctMonolith.Database.UnitOfWork;
using LctMonolith.Models.Database;
using LctMonolith.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LctMonolith.Services;

public class RuleValidationService : IRuleValidationService
{
    private readonly IUnitOfWork _uow;
    public RuleValidationService(IUnitOfWork uow) => _uow = uow;

    public async Task<bool> ValidateMissionRankRulesAsync(Guid missionId, Guid playerId)
    {
        try
        {
            var player = await _uow.Players.GetByIdAsync(playerId);
            if (player == null) return false;
            var rankRules = await _uow.MissionRankRules.Query(r => r.MissionId == missionId).Select(r => r.RankId).ToListAsync();
            if (rankRules.Count == 0) return true; // no restriction
            return rankRules.Contains(player.RankId);
        }
        catch (Exception ex) { Log.Error(ex, "ValidateMissionRankRulesAsync failed {MissionId} {PlayerId}", missionId, playerId); throw; }
    }

    public async Task<bool> ValidateRankAdvancementRulesAsync(Guid playerId, Guid targetRankId)
    {
        try
        {
            var player = await _uow.Players.GetByIdAsync(playerId);
            if (player == null) return false;
            var rank = await _uow.Ranks.GetByIdAsync(targetRankId);
            if (rank == null) return false;
            if (player.Experience < rank.ExpNeeded) return false;
            // required missions
            var missionReqs = await _uow.RankMissionRules.Query(r => r.RankId == targetRankId).Select(r => r.MissionId).ToListAsync();
            if (missionReqs.Count > 0)
            {
                var completed = await _uow.PlayerMissions.Query(pm => pm.PlayerId == playerId && pm.Completed != null).Select(pm => pm.MissionId).ToListAsync();
                if (missionReqs.Except(completed).Any()) return false;
            }
            // required skills
            var skillReqs = await _uow.RankSkillRules.Query(r => r.RankId == targetRankId).ToListAsync();
            if (skillReqs.Count > 0)
            {
                var playerSkills = await _uow.PlayerSkills.Query(ps => ps.PlayerId == playerId).ToListAsync();
                foreach (var req in skillReqs)
                {
                    var ps = playerSkills.FirstOrDefault(s => s.SkillId == req.SkillId);
                    if (ps == null || ps.Score < req.Min) return false;
                }
            }
            return true;
        }
        catch (Exception ex) { Log.Error(ex, "ValidateRankAdvancementRulesAsync failed {PlayerId}->{RankId}", playerId, targetRankId); throw; }
    }

    public async Task<IEnumerable<MissionRankRule>> GetApplicableRankRulesAsync(Guid missionId)
    {
        try { return await _uow.MissionRankRules.Query(r => r.MissionId == missionId, null, r => r.Rank).ToListAsync(); }
        catch (Exception ex) { Log.Error(ex, "GetApplicableRankRulesAsync failed {MissionId}", missionId); throw; }
    }
}
