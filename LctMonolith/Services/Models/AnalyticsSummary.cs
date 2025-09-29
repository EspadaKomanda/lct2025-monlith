namespace LctMonolith.Services.Models;

public class AnalyticsSummary
{
    public int TotalUsers { get; set; }
    public int TotalMissions { get; set; }
    public int CompletedMissions { get; set; }
    public int TotalArtifacts { get; set; }
    public int TotalStoreItems { get; set; }
    public long TotalExperience { get; set; }
    public DateTime GeneratedAtUtc { get; set; } = DateTime.UtcNow;
}
