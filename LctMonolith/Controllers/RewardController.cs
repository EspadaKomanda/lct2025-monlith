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

    /// <summary>List skill rewards configured for a mission.</summary>
    [HttpGet("mission/{missionId:guid}/skills")]
    public async Task<IActionResult> GetMissionSkillRewards(Guid missionId)
    {
        var rewards = await _rewardService.GetMissionSkillRewardsAsync(missionId);
        return Ok(rewards.Select(r => new { r.SkillId, r.Value }));
    }

    /// <summary>List item rewards configured for a mission.</summary>
    [HttpGet("mission/{missionId:guid}/items")]
    public async Task<IActionResult> GetMissionItemRewards(Guid missionId)
    {
        var rewards = await _rewardService.GetMissionItemRewardsAsync(missionId);
        return Ok(rewards.Select(r => new { r.ItemId }));
    }

    /// <summary>Check if mission rewards can be claimed by player (missionId used as rewardId).</summary>
    [HttpGet("mission/{missionId:guid}/can-claim/{playerId:guid}")]
    public async Task<IActionResult> CanClaim(Guid missionId, Guid playerId)
    {
        var can = await _rewardService.CanClaimRewardAsync(missionId, playerId);
        return Ok(new { missionId, playerId, canClaim = can });
    }

    public record ClaimRewardRequest(Guid PlayerId);

    /// <summary>Claim mission rewards if available (idempotent on already claimed).</summary>
    [HttpPost("mission/{missionId:guid}/claim")]
    public async Task<IActionResult> Claim(Guid missionId, ClaimRewardRequest req)
    {
        var can = await _rewardService.CanClaimRewardAsync(missionId, req.PlayerId);
        if (!can) return Conflict(new { message = "Rewards already claimed or mission not completed" });
        await _rewardService.DistributeMissionRewardsAsync(missionId, req.PlayerId);
        return Ok(new { missionId, req.PlayerId, status = "claimed" });
    }

    public record ForceDistributeRequest(Guid PlayerId);

    /// <summary>Admin: force distribute rewards regardless of previous state (may duplicate). Use cautiously.</summary>
    [HttpPost("mission/{missionId:guid}/force-distribute")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ForceDistribute(Guid missionId, ForceDistributeRequest req)
    {
        await _rewardService.DistributeMissionRewardsAsync(missionId, req.PlayerId);
        return Ok(new { missionId, req.PlayerId, status = "forced" });
    }
}
