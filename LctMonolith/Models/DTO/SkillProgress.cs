namespace LctMonolith.Models.DTO;

public class SkillProgress
{
    public Guid SkillId { get; set; }
    public string SkillTitle { get; set; } = string.Empty;
    public int PreviousLevel { get; set; }
    public int NewLevel { get; set; }
}
