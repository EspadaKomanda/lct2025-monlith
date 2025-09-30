namespace LctMonolith.Models.Database;

public class MissionSkillReward
{
    public Guid Id { get; set; }
    public Guid MissionId { get; set; }
    public Mission Mission { get; set; } = null!;
    public Guid SkillId { get; set; } // changed from long
    public Skill Skill { get; set; } = null!;
    public int Value { get; set; }
}
