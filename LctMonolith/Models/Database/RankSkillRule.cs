namespace LctMonolith.Models.Database;

public class RankSkillRule
{
    public Guid Id { get; set; }
    public Guid RankId { get; set; }
    public Rank Rank { get; set; } = null!;
    public Guid SkillId { get; set; }
    public Skill Skill { get; set; } = null!;
    public int Min { get; set; }
}
