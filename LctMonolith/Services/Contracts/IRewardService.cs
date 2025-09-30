using LctMonolith.Models.Database;

namespace LctMonolith.Services.Interfaces;

public interface IRewardService
{
    Task<IEnumerable<MissionSkillReward>> GetMissionSkillRewardsAsync(Guid missionId);
    Task<IEnumerable<MissionItemReward>> GetMissionItemRewardsAsync(Guid missionId);
    Task DistributeMissionRewardsAsync(Guid missionId, Guid playerId);
    Task<bool> CanClaimRewardAsync(Guid rewardId, Guid playerId);
}
