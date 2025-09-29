using LctMonolith.Models;
using LctMonolith.Services.Models;

namespace LctMonolith.Services.Contracts;

public interface IMissionService
{
    Task<Mission> CreateMissionAsync(CreateMissionModel model, CancellationToken ct = default);
    Task<IEnumerable<Mission>> GetAvailableMissionsAsync(Guid userId, CancellationToken ct = default);
    Task<UserMission> UpdateStatusAsync(Guid userId, Guid missionId, MissionStatus status, string? submissionData, CancellationToken ct = default);
}
