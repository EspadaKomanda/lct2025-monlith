using LctMonolith.Database.UnitOfWork;
using LctMonolith.Models.Database;
using LctMonolith.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LctMonolith.Services;

public class RewardService : IRewardService
{
    private readonly IUnitOfWork _uow;
    public RewardService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<MissionSkillReward>> GetMissionSkillRewardsAsync(Guid missionId)
    {
        try { return await _uow.MissionSkillRewards.Query(r => r.MissionId == missionId, null, r => r.Skill).ToListAsync(); }
        catch (Exception ex) { Log.Error(ex, "GetMissionSkillRewardsAsync failed {MissionId}", missionId); throw; }
    }

    public async Task<IEnumerable<MissionItemReward>> GetMissionItemRewardsAsync(Guid missionId)
    {
        try { return await _uow.MissionItemRewards.Query(r => r.MissionId == missionId).ToListAsync(); }
        catch (Exception ex) { Log.Error(ex, "GetMissionItemRewardsAsync failed {MissionId}", missionId); throw; }
    }

    public async Task DistributeMissionRewardsAsync(Guid missionId, Guid playerId)
    {
        try
        {
            var mission = await _uow.Missions.GetByIdAsync(missionId) ?? throw new KeyNotFoundException("Mission not found");
            var player = await _uow.Players.GetByIdAsync(playerId) ?? throw new KeyNotFoundException("Player not found");

            player.Experience += mission.ExpReward;
            player.Mana += mission.ManaReward;

            // Skill rewards
            var skillRewards = await _uow.MissionSkillRewards.Query(r => r.MissionId == missionId).ToListAsync();
            foreach (var sr in skillRewards)
            {
                var ps = await _uow.PlayerSkills.Query(x => x.PlayerId == playerId && x.SkillId == sr.SkillId).FirstOrDefaultAsync();
                if (ps == null)
                {
                    ps = new PlayerSkill { Id = Guid.NewGuid(), PlayerId = playerId, SkillId = sr.SkillId, Score = sr.Value };
                    await _uow.PlayerSkills.AddAsync(ps);
                }
                else
                {
                    ps.Score += sr.Value;
                    _uow.PlayerSkills.Update(ps);
                }
            }

            // Item rewards (store items) one each
            var itemRewards = await _uow.MissionItemRewards.Query(r => r.MissionId == missionId).ToListAsync();
            foreach (var ir in itemRewards)
            {
                var inv = await _uow.UserInventoryItems.FindAsync(player.UserId, ir.ItemId);
                if (inv == null)
                {
                    inv = new UserInventoryItem { UserId = player.UserId, StoreItemId = ir.ItemId, Quantity = 1, AcquiredAt = DateTime.UtcNow };
                    await _uow.UserInventoryItems.AddAsync(inv);
                }
                else inv.Quantity += 1;
            }

            // Mark redeemed
            var pm = await _uow.PlayerMissions.Query(pm => pm.PlayerId == playerId && pm.MissionId == missionId).FirstOrDefaultAsync();
            if (pm != null && pm.RewardsRedeemed == null)
            {
                pm.RewardsRedeemed = DateTime.UtcNow;
                _uow.PlayerMissions.Update(pm);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "DistributeMissionRewardsAsync failed {MissionId} {PlayerId}", missionId, playerId);
            throw;
        }
    }

    public async Task<bool> CanClaimRewardAsync(Guid rewardId, Guid playerId)
    {
        try
        {
            // Interpret rewardId as missionId; claim if mission completed and rewards not yet redeemed
            var pm = await _uow.PlayerMissions.Query(pm => pm.PlayerId == playerId && pm.MissionId == rewardId).FirstOrDefaultAsync();
            if (pm == null || pm.Completed == null) return false;
            return pm.RewardsRedeemed == null;
        }
        catch (Exception ex) { Log.Error(ex, "CanClaimRewardAsync failed {RewardId} {PlayerId}", rewardId, playerId); throw; }
    }
}
