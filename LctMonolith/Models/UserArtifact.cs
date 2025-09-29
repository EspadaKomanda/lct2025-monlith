namespace LctMonolith.Models;

public class UserArtifact
{
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public Guid ArtifactId { get; set; }
    public Artifact Artifact { get; set; } = null!;
    public DateTime ObtainedAt { get; set; } = DateTime.UtcNow;
}
