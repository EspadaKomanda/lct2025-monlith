using LctMonolith.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LctMonolith.Controllers;

[ApiController]
[Route("api/rewards")]
[Authorize]
public class RewardController : ControllerBase
{
    private readonly IRewardService _rewardService;

    public RewardController(IRewardService rewardService)
    {
        _rewardService = rewardService;
    }

    [HttpGet("mission/{missionId:guid}/skills")]
    public async Task<IActionResult> GetMissionSkillRewards(Guid missionId)
    {
        var rewards = await _rewardService.GetMissionSkillRewardsAsync(missionId);
        var shaped = rewards.Select(r => new { r.SkillId, r.Value });
        return Ok(shaped);
    }

    [HttpGet("mission/{missionId:guid}/items")]
    public async Task<IActionResult> GetMissionItemRewards(Guid missionId)
    {
        var rewards = await _rewardService.GetMissionItemRewardsAsync(missionId);
        var shaped = rewards.Select(r => new { r.ItemId });
        return Ok(shaped);
    }

    [HttpGet("mission/{missionId:guid}/can-claim/{playerId:guid}")]
    public async Task<IActionResult> CanClaim(Guid missionId, Guid playerId)
    {
        var can = await _rewardService.CanClaimRewardAsync(missionId, playerId);
        return Ok(new { missionId, playerId, canClaim = can });
    }

    public record ClaimRewardRequest(Guid PlayerId);

    [HttpPost("mission/{missionId:guid}/claim")]
    public async Task<IActionResult> Claim(Guid missionId, ClaimRewardRequest req)
    {
        var can = await _rewardService.CanClaimRewardAsync(missionId, req.PlayerId);
        if (!can)
        {
            return Conflict(new { message = "Rewards already claimed or mission not completed" });
        }
        await _rewardService.DistributeMissionRewardsAsync(missionId, req.PlayerId);
        return Ok(new { missionId, req.PlayerId, status = "claimed" });
    }

    public record ForceDistributeRequest(Guid PlayerId);

    [HttpPost("mission/{missionId:guid}/force-distribute")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ForceDistribute(Guid missionId, ForceDistributeRequest req)
    {
        await _rewardService.DistributeMissionRewardsAsync(missionId, req.PlayerId);
        return Ok(new { missionId, req.PlayerId, status = "forced" });
    }
}
