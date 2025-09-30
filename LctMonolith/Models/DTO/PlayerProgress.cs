using LctMonolith.Models.Database;

namespace LctMonolith.Models.DTO;

public class PlayerProgress
{
    public Guid PlayerId { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public Rank? CurrentRank { get; set; }
    public int TotalExperience { get; set; }
    public int TotalMana { get; set; }
    public int CompletedMissions { get; set; }
    public int TotalAvailableMissions { get; set; }
    public Dictionary<string, int> SkillLevels { get; set; } = new();
}
