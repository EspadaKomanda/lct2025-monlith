namespace LctMonolith.Models;

public class Competency
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public ICollection<UserCompetency> UserCompetencies { get; set; } = new List<UserCompetency>();
    public ICollection<MissionCompetencyReward> MissionRewards { get; set; } = new List<MissionCompetencyReward>();
    public ICollection<RankRequiredCompetency> RankRequirements { get; set; } = new List<RankRequiredCompetency>();
}
