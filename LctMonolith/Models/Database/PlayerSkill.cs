namespace LctMonolith.Models.Database;

public class PlayerSkill
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Player Player { get; set; } = null!;
    public Guid SkillId { get; set; }
    public Skill Skill { get; set; } = null!;
    public int Score { get; set; }
}
