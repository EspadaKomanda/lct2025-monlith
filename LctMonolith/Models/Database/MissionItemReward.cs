namespace LctMonolith.Models.Database;

public class MissionItemReward
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public Guid MissionId { get; set; }
    public required Mission Mission { get; set; }
}
