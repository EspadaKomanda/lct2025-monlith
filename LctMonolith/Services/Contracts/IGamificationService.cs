using LctMonolith.Models;
using LctMonolith.Services.Models;

namespace LctMonolith.Services.Contracts;

/// <summary>Gamification progression logic (progress, rewards, rank evaluation).</summary>
public interface IGamificationService
{
    /// <summary>Get current user progression snapshot (xp, mana, next rank requirements).</summary>
    Task<ProgressSnapshot> GetProgressAsync(Guid userId, CancellationToken ct = default);
    /// <summary>Apply mission completion rewards (xp, mana, skills, artifacts) to user.</summary>
    Task ApplyMissionCompletionAsync(Guid userId, Mission mission, CancellationToken ct = default);
    /// <summary>Re-evaluate and apply rank upgrade if requirements are met.</summary>
    Task EvaluateRankUpgradeAsync(Guid userId, CancellationToken ct = default);
}
