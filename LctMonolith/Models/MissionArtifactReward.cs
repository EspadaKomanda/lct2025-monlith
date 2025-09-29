namespace LctMonolith.Models;

public class MissionArtifactReward
{
    public Guid MissionId { get; set; }
    public Mission Mission { get; set; } = null!;
    public Guid ArtifactId { get; set; }
    public Artifact Artifact { get; set; } = null!;
}
