using LctMonolith.Models.Database;

namespace LctMonolith.Services.Models;

public class UpdateMissionStatusRequest
{
    public MissionStatus Status { get; set; }
    public string? SubmissionData { get; set; }
}
