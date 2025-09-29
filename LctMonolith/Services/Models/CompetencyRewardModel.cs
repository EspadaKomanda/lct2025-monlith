namespace LctMonolith.Services.Models;

public class CompetencyRewardModel
{
    public Guid CompetencyId { get; set; }
    public int LevelDelta { get; set; }
    public int ProgressPointsDelta { get; set; }
}
