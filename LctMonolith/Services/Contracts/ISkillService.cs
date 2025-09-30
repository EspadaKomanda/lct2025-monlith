using LctMonolith.Models.Database;

namespace LctMonolith.Services.Interfaces;

public interface ISkillService
{
    Task<Skill?> GetSkillByIdAsync(Guid skillId);
    Task<Skill?> GetSkillByTitleAsync(string title);
    Task<IEnumerable<Skill>> GetAllSkillsAsync();
    Task<Skill> CreateSkillAsync(Skill skill);
    Task<Skill> UpdateSkillAsync(Skill skill);
    Task<bool> DeleteSkillAsync(Guid skillId);
    Task<PlayerSkill> UpdatePlayerSkillAsync(Guid playerId, Guid skillId, int level);
    Task<IEnumerable<PlayerSkill>> GetPlayerSkillsAsync(Guid playerId);
    Task<int> GetPlayerSkillLevelAsync(Guid playerId, Guid skillId);
}
