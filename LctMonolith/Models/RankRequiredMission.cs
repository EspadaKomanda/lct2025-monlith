namespace LctMonolith.Models;

public class RankRequiredMission
{
    public Guid RankId { get; set; }
    public Rank Rank { get; set; } = null!;
    public Guid MissionId { get; set; }
    public Mission Mission { get; set; } = null!;
}
