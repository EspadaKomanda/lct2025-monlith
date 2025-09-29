namespace LctMonolith.Models;

public class MissionCompetencyReward
{
    public Guid MissionId { get; set; }
    public Mission Mission { get; set; } = null!;
    public Guid CompetencyId { get; set; }
    public Competency Competency { get; set; } = null!;
    public int LevelDelta { get; set; }
    public int ProgressPointsDelta { get; set; }
}
