namespace LctMonolith.Models.Database;

public class PlayerMission
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Player Player { get; set; } = null!; // removed required
    public Guid MissionId { get; set; }
    public Mission Mission { get; set; } = null!; // removed required
    public DateTime? Started { get; set; }
    public DateTime? Completed { get; set; }
    public DateTime? RewardsRedeemed { get; set; }
    public int ProgressPercent { get; set; } // 0..100
}
