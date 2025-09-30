using LctMonolith.Database.UnitOfWork;
using LctMonolith.Models.Database;
using LctMonolith.Models.DTO;
using LctMonolith.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LctMonolith.Services;

public class MissionService : IMissionService
{
    private readonly IUnitOfWork _uow;
    private readonly IRewardService _rewardService;
    private readonly IRuleValidationService _ruleValidationService;

    public MissionService(IUnitOfWork uow, IRewardService rewardService, IRuleValidationService ruleValidationService)
    {
        _uow = uow;
        _rewardService = rewardService;
        _ruleValidationService = ruleValidationService;
    }

    public async Task<Mission?> GetMissionByIdAsync(Guid missionId)
    {
        try { return await _uow.Missions.GetByIdAsync(missionId); }
        catch (Exception ex) { Log.Error(ex, "GetMissionByIdAsync failed {MissionId}", missionId); throw; }
    }

    public async Task<IEnumerable<Mission>> GetMissionsByCategoryAsync(Guid categoryId)
    {
        try { return await _uow.Missions.Query(m => m.MissionCategoryId == categoryId).ToListAsync(); }
        catch (Exception ex) { Log.Error(ex, "GetMissionsByCategoryAsync failed {Category}", categoryId); throw; }
    }

    public async Task<IEnumerable<Mission>> GetAvailableMissionsForPlayerAsync(Guid playerId)
    {
        try
        {
            var missions = await _uow.Missions.Query().ToListAsync();
            var result = new List<Mission>();
            foreach (var m in missions)
            {
                if (await IsMissionAvailableForPlayerAsync(m.Id, playerId)) result.Add(m);
            }
            return result;
        }
        catch (Exception ex) { Log.Error(ex, "GetAvailableMissionsForPlayerAsync failed {Player}", playerId); throw; }
    }

    public async Task<IEnumerable<Mission>> GetChildMissionsAsync(Guid parentMissionId)
    {
        try { return await _uow.Missions.Query(m => m.ParentMissionId == parentMissionId).ToListAsync(); }
        catch (Exception ex) { Log.Error(ex, "GetChildMissionsAsync failed {ParentMission}", parentMissionId); throw; }
    }

    public async Task<Mission> CreateMissionAsync(Mission mission)
    {
        try
        {
            mission.Id = Guid.NewGuid();
            await _uow.Missions.AddAsync(mission);
            await _uow.SaveChangesAsync();
            return mission;
        }
        catch (Exception ex) { Log.Error(ex, "CreateMissionAsync failed {Title}", mission.Title); throw; }
    }

    public async Task<Mission> UpdateMissionAsync(Mission mission)
    {
        try { _uow.Missions.Update(mission); await _uow.SaveChangesAsync(); return mission; }
        catch (Exception ex) { Log.Error(ex, "UpdateMissionAsync failed {MissionId}", mission.Id); throw; }
    }

    public async Task<bool> DeleteMissionAsync(Guid missionId)
    {
        try { var m = await _uow.Missions.GetByIdAsync(missionId); if (m == null) return false; _uow.Missions.Remove(m); await _uow.SaveChangesAsync(); return true; }
        catch (Exception ex) { Log.Error(ex, "DeleteMissionAsync failed {MissionId}", missionId); throw; }
    }

    public async Task<bool> IsMissionAvailableForPlayerAsync(Guid missionId, Guid playerId)
    {
        try
        {
            var mission = await _uow.Missions.GetByIdAsync(missionId);
            if (mission == null) return false;
            // rule validation
            if (!await _ruleValidationService.ValidateMissionRankRulesAsync(missionId, playerId)) return false;
            // already completed? then not available
            var completed = await _uow.PlayerMissions.Query(pm => pm.PlayerId == playerId && pm.MissionId == missionId && pm.Completed != null).AnyAsync();
            if (completed) return false;
            // if parent mission required ensure parent completed
            if (mission.ParentMissionId.HasValue)
            {
                var parentDone = await _uow.PlayerMissions.Query(pm => pm.PlayerId == playerId && pm.MissionId == mission.ParentMissionId && pm.Completed != null).AnyAsync();
                if (!parentDone) return false;
            }
            return true;
        }
        catch (Exception ex) { Log.Error(ex, "IsMissionAvailableForPlayerAsync failed {MissionId} {PlayerId}", missionId, playerId); throw; }
    }

    public async Task<MissionCompletionResult> CompleteMissionAsync(Guid missionId, Guid playerId, object? missionProof = null)
    {
        try
        {
            if (!await IsMissionAvailableForPlayerAsync(missionId, playerId))
            {
                return new MissionCompletionResult { Success = false, Message = "Mission not available" };
            }
            var mission = await _uow.Missions.GetByIdAsync(missionId) ?? throw new KeyNotFoundException("Mission not found");
            var player = await _uow.Players.GetByIdAsync(playerId) ?? throw new KeyNotFoundException("Player not found");

            // snapshot skill levels before
            var skillRewards = await _uow.MissionSkillRewards.Query(r => r.MissionId == missionId).ToListAsync();
            var beforeSkills = await _uow.PlayerSkills.Query(ps => ps.PlayerId == playerId).ToListAsync();

            // mark PlayerMission
            var pm = await _uow.PlayerMissions.Query(x => x.PlayerId == playerId && x.MissionId == missionId).FirstOrDefaultAsync();
            if (pm == null)
            {
                pm = new PlayerMission { Id = Guid.NewGuid(), PlayerId = playerId, MissionId = missionId, Started = DateTime.UtcNow };
                await _uow.PlayerMissions.AddAsync(pm);
            }
            pm.Completed = DateTime.UtcNow;
            pm.ProgressPercent = 100;
            await _uow.SaveChangesAsync();

            var prevExp = player.Experience;
            var prevMana = player.Mana;

            // distribute rewards (XP/Mana/Skills/Items)
            await _rewardService.DistributeMissionRewardsAsync(missionId, playerId);
            await _uow.SaveChangesAsync();

            // build skill progress
            var afterSkills = await _uow.PlayerSkills.Query(ps => ps.PlayerId == playerId).ToListAsync();
            var skillProgress = new List<SkillProgress>();
            foreach (var r in skillRewards)
            {
                var before = beforeSkills.FirstOrDefault(s => s.SkillId == r.SkillId)?.Score ?? 0;
                var after = afterSkills.FirstOrDefault(s => s.SkillId == r.SkillId)?.Score ?? before;
                if (after != before)
                {
                    var skill = await _uow.Skills.GetByIdAsync(r.SkillId);
                    skillProgress.Add(new SkillProgress { SkillId = r.SkillId, SkillTitle = skill?.Title ?? string.Empty, PreviousLevel = before, NewLevel = after });
                }
            }

            return new MissionCompletionResult
            {
                Success = true,
                Message = "Mission completed",
                ExperienceGained = player.Experience - prevExp,
                ManaGained = player.Mana - prevMana,
                SkillsProgress = skillProgress,
                UnlockedMissions = (await _uow.Missions.Query(m => m.ParentMissionId == missionId).Select(m => m.Id).ToListAsync())
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "CompleteMissionAsync failed {MissionId} {PlayerId}", missionId, playerId);
            throw;
        }
    }
}
