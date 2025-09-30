namespace LctMonolith.Models.Database;

public class Skill
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public ICollection<MissionSkillReward> MissionSkillRewards { get; set; } = new List<MissionSkillReward>();
    public ICollection<RankSkillRule> RankSkillRules { get; set; } = new List<RankSkillRule>();
    public ICollection<PlayerSkill> PlayerSkills { get; set; } = new List<PlayerSkill>();
}
