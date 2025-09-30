namespace LctMonolith.Models.Database;

public class RankMissionRule
{
    public Guid Id { get; set; }
    public Guid RankId { get; set; }
    public Rank Rank { get; set; } = null!;
    public Guid MissionId { get; set; }
    public Mission Mission { get; set; } = null!;
}
