using LctMonolith.Models.Database;
using LctMonolith.Models.DTO;

namespace LctMonolith.Services.Interfaces;

public interface IMissionService
{
    Task<Mission?> GetMissionByIdAsync(Guid missionId);
    Task<IEnumerable<Mission>> GetMissionsByCategoryAsync(Guid categoryId);
    Task<IEnumerable<Mission>> GetAvailableMissionsForPlayerAsync(Guid playerId);
    Task<IEnumerable<Mission>> GetChildMissionsAsync(Guid parentMissionId);
    Task<Mission> CreateMissionAsync(Mission mission);
    Task<Mission> UpdateMissionAsync(Mission mission);
    Task<bool> DeleteMissionAsync(Guid missionId);
    Task<bool> IsMissionAvailableForPlayerAsync(Guid missionId, Guid playerId);
    Task<MissionCompletionResult> CompleteMissionAsync(Guid missionId, Guid playerId, object? missionProof = null);
}
