namespace LctMonolith.Models.Database;

public class Rank
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int ExpNeeded { get; set; }

    public ICollection<Player> Players { get; set; } = new List<Player>();
    public ICollection<MissionRankRule> MissionRankRules { get; set; } = new List<MissionRankRule>();
    public ICollection<RankMissionRule> RankMissionRules { get; set; } = new List<RankMissionRule>();
    public ICollection<RankSkillRule> RankSkillRules { get; set; } = new List<RankSkillRule>();
}
