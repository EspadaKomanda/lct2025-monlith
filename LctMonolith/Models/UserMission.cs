namespace LctMonolith.Models;

public class UserMission
{
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public Guid MissionId { get; set; }
    public Mission Mission { get; set; } = null!;
    public MissionStatus Status { get; set; } = MissionStatus.Available;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string? SubmissionData { get; set; }
}
