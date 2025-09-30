namespace LctMonolith.Models.Database;

public class PlayerMission
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public required Player Player { get; set; }
    public Guid MissionId { get; set; }
    public required Mission Mission { get; set; }
    public DateTime? Started { get; set; }
    public DateTime? Completed { get; set; }
    public DateTime? RewardsRedeemed { get; set; }
}
