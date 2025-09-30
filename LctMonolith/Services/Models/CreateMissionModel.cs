using LctMonolith.Models.Database;

namespace LctMonolith.Services.Models;

public class CreateMissionModel
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? Branch { get; set; }
    public MissionCategory Category { get; set; }
    public Guid? MinRankId { get; set; }
    public int ExperienceReward { get; set; }
    public int ManaReward { get; set; }
    public List<CompetencyRewardModel> CompetencyRewards { get; set; } = new();
}
