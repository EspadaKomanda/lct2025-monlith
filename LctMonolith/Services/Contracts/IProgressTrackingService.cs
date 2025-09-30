using LctMonolith.Models.Database;
using LctMonolith.Models.DTO;

namespace LctMonolith.Services.Interfaces;

public interface IProgressTrackingService
{
    Task<PlayerMission> StartMissionAsync(Guid missionId, Guid playerId);
    Task<PlayerMission> UpdateMissionProgressAsync(Guid playerMissionId, int progressPercentage, object? proof = null);
    Task<PlayerMission> CompleteMissionAsync(Guid playerMissionId, object? proof = null);
    Task<IEnumerable<PlayerMission>> GetPlayerMissionsAsync(Guid playerId);
    Task<PlayerMission?> GetPlayerMissionAsync(Guid playerId, Guid missionId);
    Task<PlayerProgress> GetPlayerOverallProgressAsync(Guid playerId);
}
