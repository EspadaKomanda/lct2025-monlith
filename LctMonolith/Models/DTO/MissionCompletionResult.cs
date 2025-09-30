namespace LctMonolith.Models.DTO;

public class MissionCompletionResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int ExperienceGained { get; set; }
    public int ManaGained { get; set; }
    public List<SkillProgress> SkillsProgress { get; set; } = new();
    public List<Guid> UnlockedMissions { get; set; } = new();
}
