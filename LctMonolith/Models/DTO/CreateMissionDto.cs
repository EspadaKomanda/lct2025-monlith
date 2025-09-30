namespace LctMonolith.Models.DTO;

public class CreateMissionDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid MissionCategoryId { get; set; }
    public Guid? ParentMissionId { get; set; }
    public int ExpReward { get; set; }
    public int ManaReward { get; set; }
}

