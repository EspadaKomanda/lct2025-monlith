using LctMonolith.Models.Database;

namespace LctMonolith.Services.Interfaces;

public interface IRuleValidationService
{
    Task<bool> ValidateMissionRankRulesAsync(Guid missionId, Guid playerId);
    Task<bool> ValidateRankAdvancementRulesAsync(Guid playerId, Guid targetRankId);
    Task<IEnumerable<MissionRankRule>> GetApplicableRankRulesAsync(Guid missionId);
}
