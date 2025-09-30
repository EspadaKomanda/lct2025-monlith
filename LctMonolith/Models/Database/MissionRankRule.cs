namespace LctMonolith.Models.Database;

public class MissionRankRule
{
    public Guid Id { get; set; }
    public Guid MissionId { get; set; }
    public Mission Mission { get; set; } = null!;
    public Guid RankId { get; set; }
    public Rank Rank { get; set; } = null!;
}
