namespace LctMonolith.Models;

public class Mission
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? Branch { get; set; }
    public MissionCategory Category { get; set; }
    public Guid? MinRankId { get; set; }
    public Rank? MinRank { get; set; }
    public int ExperienceReward { get; set; }
    public int ManaReward { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<MissionCompetencyReward> CompetencyRewards { get; set; } = new List<MissionCompetencyReward>();
    public ICollection<MissionArtifactReward> ArtifactRewards { get; set; } = new List<MissionArtifactReward>();
    public ICollection<UserMission> UserMissions { get; set; } = new List<UserMission>();
    public ICollection<RankRequiredMission> RanksRequiring { get; set; } = new List<RankRequiredMission>();
}
